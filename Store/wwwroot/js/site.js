// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// add item to cart
function addItemToCart(id, name, price) {
    const item = { Id: id, Name: name, Price: price };
    addToCart(item);
}

// add item to cart
function addToCart(item) {
    fetch('/CartItems/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(updatedCart => {
            updateCartDisplay(updatedCart);
        })
        .catch(error => console.error('Error:', error));
}

// update cart display with new cart data
function updateCartDisplay(cart) {
    const cartContainer = document.getElementById('cart');

    if (cart.length === 0) {
        cartContainer.innerHTML = '<p>Your cart is empty</p>';
    } else {
        const cartItemsContainer = document.getElementById('cart-items');
        cartItemsContainer.innerHTML = '';
        let totalPrice = 0;

        cart.forEach(item => {
            const li = document.createElement('li');
            li.className = 'd-flex align-items-center mb-4';

            const itemNamePriceDiv = document.createElement('div');
            itemNamePriceDiv.className = 'me-auto';
            itemNamePriceDiv.textContent = `${item.name} - $${item.price.toFixed(2)}`;

            const quantityDiv = document.createElement('div');
            quantityDiv.className = 'mx-3';
            quantityDiv.innerHTML = 'Quantity: ';

            const quantityInput = document.createElement('input');
            quantityInput.type = 'number';
            quantityInput.min = '1';
            quantityInput.value = item.quantity;
            quantityInput.className = 'form-control d-inline-block w-25';
            quantityInput.onchange = () => updateQuantity(item.id, quantityInput.value);

            quantityDiv.appendChild(quantityInput);

            const removeButton = document.createElement('button');
            removeButton.textContent = 'Remove';
            removeButton.className = 'btn btn-danger mb-lg-4';
            removeButton.onclick = () => removeFromCart(item.id);

            li.appendChild(itemNamePriceDiv);
            li.appendChild(quantityDiv);
            li.appendChild(removeButton);

            cartItemsContainer.appendChild(li);

            totalPrice += item.price * item.quantity;
        });

        document.getElementById('total-price').textContent = totalPrice.toFixed(2);
    }
}



// remove item from cart
function removeFromCart(id) {
    fetch(`/CartItems/Remove?id=${id}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(cart => {
            updateCartDisplay(cart);
        })
        .catch(error => console.error('Error removing item:', error));
}

// update quantity of item in cart
function updateQuantity(id, quantity) {
    fetch(`/CartItems/UpdateQuantity?id=${id}&quantity=${quantity}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(cart => {
            updateCartDisplay(cart);
        })
        .catch(error => console.error('Error updating quantity:', error));
}


const onSearch = () => {
    const input = document.querySelector("#searchTerm");
    const filter = input.value.toUpperCase();
    const searchBy = document.querySelector("#searchBy").value;
    const rows = document.querySelectorAll("#ordersTable tbody tr");

    rows.forEach(row => {
        let text;
        if (searchBy === "User") {
            text = row.querySelector(".username").textContent.toUpperCase();
        } else if (searchBy === "Status") {
            text = row.querySelector(".status").textContent.toUpperCase();
        }
        row.style.display = text.includes(filter) ? "" : "none";
    });
}