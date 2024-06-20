using Microsoft.AspNetCore.Mvc.RazorPages;
using Store.Models;
using Store.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Store.Views.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order? Order { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            OrderProducts = await _context.orderProducts
                .Include(op => op.Product)
                .Where(op => op.OrderId == id)
                .ToListAsync();

            return Page();
        }

        // Adding the OnPostAsync method to handle the deletion
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
