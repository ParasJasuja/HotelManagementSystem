//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using HotelManagementSystem.Data;
//using HotelManagementSystem.Models;
//using Microsoft.AspNetCore.Authorization;

//namespace HotelManagementSystem.Controllers
//{
//    public class OrdersController : Controller
//    {
//        private readonly HotelManagementSystemDbContext _context;

//        public OrdersController(HotelManagementSystemDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Orders
//        public async Task<IActionResult> Index()
//        {
//            var hotelManagementSystemDbContext = _context.Orders.Include(o => o.Menu).Include(o => o.User);
//            return View(await hotelManagementSystemDbContext.ToListAsync());
//        }

//        // GET: Orders/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Orders == null)
//            {
//                return NotFound();
//            }

//            var order = await _context.Orders
//                .Include(o => o.Menu)
//                .Include(o => o.User)
//                .FirstOrDefaultAsync(m => m.OrderId == id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            return View(order);
//        }

//        // GET: Orders/Create
//        public IActionResult Create()
//        {
//            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId");
//            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
//            return View();
//        }

//        // POST: Orders/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Authorize(Roles = "User")]
//        public async Task<IActionResult> Create([Bind("OrderId,MenuId,UserId,OrderStatus,PaymentStatus,OrderTime,Menu,User")] Order order)
//        {
//            Console.WriteLine(order);
//            if (ModelState.IsValid)
//            {
//                _context.Add(order);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", order.MenuId);
//            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
//            return View(order);
//        }

//        // GET: Orders/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Orders == null)
//            {
//                return NotFound();
//            }

//            var order = await _context.Orders.FindAsync(id);
//            if (order == null)
//            {
//                return NotFound();
//            }
//            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", order.MenuId);
//            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
//            return View(order);
//        }

//        // POST: Orders/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("OrderId,MenuId,UserId,OrderStatus,PaymentStatus,OrderTime")] Order order)
//        {
//            if (id != order.OrderId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(order);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!OrderExists(order.OrderId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", order.MenuId);
//            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
//            return View(order);
//        }

//        // GET: Orders/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Orders == null)
//            {
//                return NotFound();
//            }

//            var order = await _context.Orders
//                .Include(o => o.Menu)
//                .Include(o => o.User)
//                .FirstOrDefaultAsync(m => m.OrderId == id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            return View(order);
//        }

//        // POST: Orders/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Orders == null)
//            {
//                return Problem("Entity set 'HotelManagementSystemDbContext.Orders'  is null.");
//            }
//            var order = await _context.Orders.FindAsync(id);
//            if (order != null)
//            {
//                _context.Orders.Remove(order);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool OrderExists(int id)
//        {
//          return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
//        }
//    }
//}
