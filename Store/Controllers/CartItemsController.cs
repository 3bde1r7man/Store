using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Models;

namespace Store.Controllers
{
    public class CartItemsController : Controller
    {
        //private static List<CartItem> cart = new List<CartItem>();
        // make it aisoulated from each user for a user has his own cart as a map username to cart
        private static Dictionary<string, List<CartItem>> cart = new Dictionary<string, List<CartItem>>();




        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CartItems
        public IActionResult Index()
		{
            if(cart.ContainsKey(User.Identity.Name))
            {
                return View(cart[User.Identity.Name]);
            }
            else
            {
                return View(new List<CartItem>());
            }

		}

        [HttpPost]
        public IActionResult Add([FromBody] CartItem item)
        {
            if (item != null)
            {
                if (cart.ContainsKey(User.Identity.Name))
                {
                    var temp = cart[User.Identity.Name].Find(x => x.Id == item.Id);
                    if (temp != null)
                    {
                        temp.Quantity++;
                    }
                    else
                    {
                        cart[User.Identity.Name].Add(item);
                    }
                }
                else
                {
                    cart.Add(User.Identity.Name, new List<CartItem> { item });
                }
            }
            return Json(cart[User.Identity.Name]);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var item = cart[User.Identity.Name].FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Quantity = quantity;
            }
            return Json(cart[User.Identity.Name]);
        }



        [HttpPost]
        public IActionResult Remove(int id)
        {
            var item = cart[User.Identity.Name].FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                cart[User.Identity.Name].Remove(item);
            }
            return Json(cart[User.Identity.Name]);
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
                TotalPrice = cart[User.Identity.Name].Sum(x => x.Price * x.Quantity)
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

            cart[User.Identity.Name].Clear();
            ViewBag.SuccessMessage = "Order has been placed successfully";
            return View("Index", cart[User.Identity.Name]);
        }

        private List<OrderProduct> createOrderProductList(Order order)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            var products = _context.Products.ToList();
            foreach (var item in cart[User.Identity.Name])
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
