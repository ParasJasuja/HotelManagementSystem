using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly HotelManagementSystemDbContext _context;

        public AdminController(HotelManagementSystemDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllAdmins()
        {
            return _context.AdminUsers != null ?
                        View(await _context.AdminUsers.ToListAsync()) :
                        Problem("Entity set 'HotelManagementSystemDbContext.AdminUsers'  is null.");
        }
        [AllowAnonymous]
        public IActionResult Index(string ReturnUrl = "Dashboard")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(ReturnUrl);
            }
                AdminViewModel adminUser = new AdminViewModel();
            adminUser.ReturnUrl = ReturnUrl;
            return View(adminUser);
        }
        [HttpPost]
        public async Task<IActionResult> Index([Bind("Username,HashPassword,ReturnUrl")] AdminViewModel admin)
        {
            if (ModelState.IsValid)
            {
                var userFound = _context.AdminUsers.Where(u => u.Username == admin.Username && u.HashPassword == admin.HashPassword).FirstOrDefault();
                if (userFound != null)
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userFound.AdminId)),
                        new Claim(ClaimTypes.Role, "Admin")
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = false
                    });
                    return LocalRedirect(admin.ReturnUrl);
                }
                else
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(admin);
                }
            }
            ViewBag.Message = "Problem";
            return View(admin);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdminUsers == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,Username,HashPassword")] AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(adminUser);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdminUsers == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser == null)
            {
                return NotFound();
            }
            return View(adminUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,Username,HashPassword")] AdminUser adminUser)
        {
            if (id != adminUser.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminUserExists(adminUser.AdminId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Dashboard");
            }
            return View(adminUser);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdminUsers == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdminUsers == null)
            {
                return Problem("Entity set 'HotelManagementSystemDbContext.AdminUsers'  is null.");
            }
            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser != null)
            {
                _context.AdminUsers.Remove(adminUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        //////////// Booking ///////////////////////
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Bookings()
        {
            var hotelManagementSystemDbContext = _context.Bookings.Include(b => b.RoomNoNavigation).Include(b => b.User);
            return View(await hotelManagementSystemDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> BookingDetails(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.RoomNoNavigation)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> BookingEdit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["RoomNo"] = new SelectList(_context.Rooms, "RoomNo", "RoomNo", booking.RoomNo);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", booking.UserId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingEdit(int id, [Bind("BookingId,RoomNo,UserId,BookingStatus,PaymentStatus,DurationStart,DurationEnd,BookingTime")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Bookings");
            }
            ViewData["RoomNo"] = new SelectList(_context.Rooms, "RoomNo", "RoomNo", booking.RoomNo);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", booking.UserId);
            return View(booking);
        }

        ////////////////////// Rooms /////////////////////////////
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Rooms()
        {
            return _context.Rooms != null ?
                        View(await _context.Rooms.ToListAsync()) :
                        Problem("Entity set 'HotelManagementSystemDbContext.Rooms'  is null.");
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> RoomDetails(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomNo == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        public IActionResult RoomCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoomCreate([Bind("RoomNo,RoomType,RoomDescription,RoomCharges,IsOpen")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Rooms");
            }
            return View(room);
        }

        public async Task<IActionResult> RoomEdit(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> RoomEdit(int id, [Bind("RoomNo,RoomType,RoomDescription,RoomCharges,IsOpen")] Room room)
        {
            if (id != room.RoomNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Rooms");
            }
            return View(room);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> RoomDelete(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomNo == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        
        [HttpPost, ActionName("RoomDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoomDeleteConfirmed(int id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'HotelManagementSystemDbContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Rooms");
        }

        //////////////// Menus //////////////////////////////
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Menus()
        {
            return _context.Menus != null ?
                        View(await _context.Menus.ToListAsync()) :
                        Problem("Entity set 'HotelManagementSystemDbContext.Menus'  is null.");
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> MenuDetails(int? id)
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

        [Authorize(Roles = "Admin")]

        public IActionResult MenuCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuCreate([Bind("MenuId,MenuType,MenuDescription,MenuPrice,MenuName")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Menus");
            }
            return View(menu);
        }


        public async Task<IActionResult> MenuEdit(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuEdit(int id, [Bind("MenuId,MenuType,MenuDescription,MenuPrice,MenuName")] Menu menu)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Menus");
            }
            return View(menu);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> MenuDelete(int? id)
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

        [HttpPost, ActionName("MenuDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MenuDeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'HotelManagementSystemDbContext.Menus'  is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Menus");
        }
        //////////// Orders /////////////
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Orders()
        {
            var hotelManagementSystemDbContext = _context.Orders.Include(o => o.Menu).Include(o => o.User);
            return View(await hotelManagementSystemDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> OrderDetails(int? id)
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

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> OrderEdit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderEdit(int id, [Bind("OrderId,MenuId,UserId,OrderStatus,PaymentStatus,OrderTime")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Orders");
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", order.MenuId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

        private bool MenuExists(int id)
        {
            return (_context.Menus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }
        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
        private bool AdminUserExists(int id)
        {
            return (_context.AdminUsers?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }
        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomNo == id)).GetValueOrDefault();
        }
    }
}