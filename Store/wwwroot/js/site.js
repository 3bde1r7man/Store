// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addItemToCart(id, name, price) {
    const item = { Id: id, Name: name, Price: price };
    addToCart(item);
}

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

function updateCartDisplay(cart) {
    const cartitems = document.getElementById('cart-items');
    cartitems.innerHTML = '';
    let totalPrice = 0;

    cart.forEach(item => {
        const li = document.createElement('li');
        li.textContent = `${item.name} - $${item.price.toFixed(2)}`;

        const quantityDiv = document.createElement('div');
        quantityDiv.textContent = 'Quantity: ';

        const quantityInput = document.createElement('input');
        quantityInput.type = 'number';
        quantityInput.min = '1';
        quantityInput.value = item.quantity;
        quantityInput.onchange = () => updateQuantity(item.id, quantityInput.value);

        quantityDiv.appendChild(quantityInput);
        li.appendChild(quantityDiv);

        const removeButton = document.createElement('button');
        removeButton.textContent = 'Remove';
        removeButton.className = 'btn btn-danger mb-lg-4'; // Add btn classes
        removeButton.onclick = () => removeFromCart(item.id);

        li.appendChild(removeButton);
        cartitems.appendChild(li);

        totalPrice += item.price * item.quantity;
    });

    document.getElementById('total-price').textContent = totalPrice.toFixed(2);
}


// Assuming removeFromCart function is defined somewhere to handle removing items from the cart
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

// Function to update quantity
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
