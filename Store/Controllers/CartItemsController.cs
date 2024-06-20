using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models;

namespace Store.Controllers
{
    public class CartItemsController : Controller
    {
        private static List<CartItem> cart = new List<CartItem>();

        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CartItems
        public IActionResult Index()
		{
			return View(cart);
		}

        [HttpPost]
        public IActionResult Add([FromBody] CartItem item)
        {

            if (item != null)
            {
                var temp = cart.Find(x => x.Id == item.Id);
                if (temp != null)
                {
                    temp.Quantity++;
                }
                else
                {
                    cart.Add(item);
                }

            }
            return Json(cart);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var item = cart.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Quantity = quantity;
            }
            return Json(cart);
        }



        [HttpPost]
        public IActionResult Remove(int id)
        {
            var item = cart.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            return Json(cart);
        }

        [HttpPost]
        public IActionResult Checkout()
        {
           

            var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null)
            {
                ViewBag.ErrorMessage = "You must be logged in to checkout";
                return View("Index", cart);
            }
            
            Order order = new Order
            {
                Date = DateTime.Now,
                UserId = user.Id,
                User = user,
                Status = "Pending",
                TotalPrice = cart.Sum(x => x.Price * x.Quantity)
            };
            if (user.Balance < order.TotalPrice)
            {
                ViewBag.ErrorMessage = "Insufficient balance";
                return View("Index", cart);
            }
            user.Balance -= order.TotalPrice;
            var orderProducts = createOrderProductList(order);
            _context.Orders.Add(order);
            _context.orderProducts.AddRange(orderProducts);
            _context.SaveChanges();
            //var tempOrder = _context.Orders.Last(x => x.UserId == user.Id);
            
            cart.Clear();
            ViewBag.SuccessMessage = "Order has been placed successfully";
            return View("Index", cart);
        }

        private List<OrderProduct> createOrderProductList(Order order)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            var products = _context.Products.ToList();
            foreach (var item in cart)
            {
                orderProducts.Add(new OrderProduct
                {
                    Order = order,
                    Product = products.Find(x=> x.Id == item.Id),
                    Quantity = item.Quantity
                });
            }
            return orderProducts;
        }
    }
}
