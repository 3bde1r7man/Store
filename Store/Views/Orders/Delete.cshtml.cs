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


        public Order? Order { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }

        
    }
}
