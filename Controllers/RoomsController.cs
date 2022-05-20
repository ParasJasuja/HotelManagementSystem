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
    public class RoomsController : Controller
    {
        private readonly HotelManagementSystemDbContext _context;

        public RoomsController(HotelManagementSystemDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
         
        public async Task<IActionResult> Index()
        {
              return _context.Rooms != null ? 
                          View(await _context.Rooms.ToListAsync()) :
                          Problem("Entity set 'HotelManagementSystemDbContext.Rooms'  is null.");
        }

        // GET: Rooms/Details/5
         
        public async Task<IActionResult> Details(int? id)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomNo,RoomType,RoomDescription,RoomCharges,IsOpen")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        [ActionName("BookRoom")]
        public async Task<IActionResult> Book(int ?id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }
            var UserId = Int32.Parse(User.Claims.First().Value);
            var booking = new Booking();
            booking.RoomNo = (int)id;
            booking.UserId = (int)UserId;
            booking.DurationStart = DateTime.Now;
            booking.DurationEnd = DateTime.Now;
            
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         
        public async Task<IActionResult> BookRoom([Bind("RoomNo,UserId,DurationStart,DurationEnd,BookingTime")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyBookings");
            }
            else
            {
                ViewBag.Message = Convert.ToString(booking);
            }
            return View(booking);
        }
        public async Task<IActionResult> MyBookings()
        {
            var hotelManagementSystemDbContext = _context.Bookings.Include(b => b.RoomNoNavigation).Include(b => b.User).Where(m => m.UserId == Int32.Parse(User.Claims.First().Value));
            return View(await hotelManagementSystemDbContext.ToListAsync());
        }
        public async Task<IActionResult> MyBookingDetails(int? id)
        {
            Console.WriteLine(id);
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.RoomNoNavigation)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == (int)id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        public async Task<IActionResult> CancelMyBooking(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelMyBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'HotelManagementSystemDbContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.BookingStatus = "Canceled";
                _context.Bookings.Update(booking);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool RoomExists(int id)
        {
          return (_context.Rooms?.Any(e => e.RoomNo == id)).GetValueOrDefault();
        }
    }
}
