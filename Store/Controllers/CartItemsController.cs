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
                // if the item is already there  update the quantity
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
            cart.Clear();
            return Json(new {success = true});
        }
    }
}
