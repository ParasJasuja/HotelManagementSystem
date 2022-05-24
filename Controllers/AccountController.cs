using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelManagementSystemDbContext _context;
        public AccountController(HotelManagementSystemDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,FirstName,LastName,Email,PermanentAddress,Gender,PhoneNumber,HashPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Login");
            }
            return View(user);
        }
        [AllowAnonymous]

        public IActionResult Login(string ReturnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(ReturnUrl);
            }
            LoginViewModel objLoginModel = new LoginViewModel();
            objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }
        [HttpPost]

        public async Task<IActionResult> Login([Bind("Email,HashPassword,ReturnUrl,RememberLogin")] LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userFound = _context.Users.Where(u => u.Email == user.Email && u.HashPassword == user.HashPassword).FirstOrDefault();
                if (userFound != null)
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userFound.UserId)),
                        new Claim(ClaimTypes.Name, userFound.FirstName+" "+userFound.LastName),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = user.RememberLogin
                    });
                    return LocalRedirect(user.ReturnUrl);
                }
                else
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(user);
                }
            }
            ViewBag.Message = "Problem";
            return View(user);
        }

        [Authorize(Roles = "User")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Email,PermanentAddress,Gender,EmailConfirmed,PhoneNoConfirmed,PhoneNumber,HashPassword")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Profile", new { id = user.UserId});
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/");
        }
        [Authorize(Roles = "User")]

        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
