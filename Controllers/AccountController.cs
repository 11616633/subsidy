using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SubsidyReconciliation.Models;
using SubsidyReconciliation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAntiforgery _antiforgery;

        public AccountController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IAntiforgery antiforgery)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _antiforgery = antiforgery;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    APIConsumtionService aPIConsumptionService = new APIConsumtionService();
                    var responseResult = aPIConsumptionService.VerifyLogin(model.username, model.password);

                    if (responseResult == null || string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password))
                    {
                        ModelState.AddModelError("", "Invalid Username or Password!");
                        return View(model);
                    }

                    if (responseResult.DeptId == "67" || responseResult.DeptId == "22" || responseResult.DeptId == "51")
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.username),
                            new Claim("Department", responseResult.DeptId)
                        };

                        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

                        HttpContext.Session.SetString("Username", model.username);
                        _httpContextAccessor.HttpContext.Session.SetString("ActiveDirectoryUserID", responseResult.Username.ToUpper());

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You are not authorized to log in.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            HttpContext.Session.Clear();
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
            //return Redirect("");
        }
    }
}
