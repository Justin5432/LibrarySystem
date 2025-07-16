﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using test2.Models;
using test2.Areas.Frontend.Models;

namespace test2.Areas.Frontend.Controllers
{
    [Area("Frontend")]
    public class AccessController : Controller
    {
        #region field
        private readonly Test2Context _context;
        #endregion

        #region constructor
        public AccessController(Test2Context context) { _context = context; }
        #endregion

        #region action
        [HttpGet]
        public IActionResult LoginC() { return View(new Guest()); }
        [HttpGet]
        public IActionResult LoginM() { return View(new Guest()); }
        [HttpPost]
        public async Task<IActionResult> AccessCard(Guest model)
        {
            if (!ModelState.IsValid) { return View("LoginM", model); }

            var guest = await _context.Clients.FirstOrDefaultAsync(x => x.CAccount == model.Account);

            if (guest == null || guest.CPassword != model.Password)
            {
                ModelState.AddModelError("", "帳號或密碼不正確");
                return View("LoginM", model);
            }

            var guestClaim = new List<Claim>{
                new Claim(ClaimTypes.Name, guest.CAccount),
                new Claim(ClaimTypes.NameIdentifier, guest.CId.ToString()),
                new Claim("Name", guest.CName),
                new Claim("Permission", guest.Permission.ToString())
            };

            if (guest.Permission == 2) { guestClaim.Add(new Claim(ClaimTypes.Role, "Admin")); }
            if (guest.Permission == 3) { guestClaim.Add(new Claim(ClaimTypes.Role, "SuperAdmin")); }

            var guessAccess = new ClaimsIdentity(guestClaim, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(guessAccess));

            if (guest.Permission < 2) { return RedirectToAction("Index", "Home", new { area = "Frontend" }); }

            return RedirectToAction("Index", "Manage", new { area = "Backend" });
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home", new { area = "Frontend" });
        }
        #endregion
    }
}