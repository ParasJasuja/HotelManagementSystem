using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementSystem.Controllers
{
    [Authorize(Roles = "User")]
    public class MenusController : Controller
    {
        private readonly HotelManagementSystemDbContext _context;

        public MenusController(HotelManagementSystemDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.Menus != null ? 
                          View(await _context.Menus.ToListAsync()) :
                          Problem("Entity set 'HotelManagementSystemDbContext.Menus'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }
        public async Task<IActionResult> MyOrders()
        {
            var hotelManagementSystemDbContext = _context.Orders.Include(o => o.Menu).Include(o => o.User).Where(m => m.UserId == Int32.Parse(User.Claims.First().Value));
            return View(await hotelManagementSystemDbContext.ToListAsync());
        }
        public IActionResult Order(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }
            var UserId = Int32.Parse(User.Claims.First().Value);
            var order = new Order();
            order.MenuId = (int)id;
            order.UserId = (int)UserId;
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order([Bind("MenuId,UserId,Quantity")] Order order)
        {
            Console.WriteLine(order);
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyOrders");
            }
            return View(order);
        }
        public async Task<IActionResult> MyOrderDetails(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Menu)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public async Task<IActionResult> CancelMyOrder(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Menu)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelMyOrder(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'HotelManagementSystemDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = "Canceled";
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyOrders");
        }

        private bool MenuExists(int id)
        {
          return (_context.Menus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }
    }
}
