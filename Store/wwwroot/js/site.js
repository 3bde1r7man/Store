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

function updateQuantity(itemId, newQuantity) {
    fetch('/CartItems/UpdateQuantity?id=' + itemId + '&quantity=' + newQuantity, { method: 'POST' })
        .then(response => response.json())
        .then(updatedCart => {
            updateCartDisplay(updatedCart);
        })
        .catch(error => console.error('Error:', error));
   
}




async function removeFromCart(id) {
    const response = await fetch('/CartItems/Remove/${id}', { method: 'Post' });
    const updatedCart = await response.json();
    updateCartDisplay(updatedCart);
}

function updateCartDisplay(cart) {
    const cartitems = document.getElementById('cart-items');
    cartitems.innerHTML = '';
    let totalPrice = 0;
    cart.forEach(item => {
        const li = document.createElement('li');
        li.textContent = '${item.name} - $${item.price}';
        const removeButton = document.createElement('button');
        removeButton.textContent = 'Remove';
        removeButton.onclick = () => removeFromCart(item.id);
        li.appendChild(removeButton);
        cartitems.appendChild(li);
        totalPrice += item.price * item.quantity;
    });
    document.getElementById('total-price').textContent = totalPrice.toFixed(2);
}