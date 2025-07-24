
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;
using WebApplication1.UserRepository.Repositories;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDTO user, [FromServices] IUserRepository userRepository, [FromServices] IPasswordHasher passwordHasher)
        {
            if (!(await userRepository.CheckEmail(user.Email)))
            {
                ModelState.AddModelError("", "Пользователь с такой электронной почтой уже зарегистрирован!");
                return View();
            }
            string pattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";

            if (!Regex.IsMatch(user.Email, pattern, RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("", "Введите корректный email!");
                return View();
            }

            var hashedPassword = passwordHasher.Generate(user.Password);

            user.Password = hashedPassword;
            user.Role = "student";
 
            await userRepository.Add(user);

            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] IPasswordHasher passwordHasher,
            [FromServices] IJwtProvider jwtProvider)            
        {
            var user = await userRepository.GetByEmail(request.Email);

            if (user != null)
            {
                var result = passwordHasher.Verify(request.Password, user.Password);

                if (!result)
                {
                    ModelState.AddModelError("", "Неправильная электронная почта или пароль!");
                    return View();
                }

                string accessToken = jwtProvider.GenerateAccessToken(user);
                string refreshToken = jwtProvider.GenerateRefreshToken();

                await userRepository.SaveRefreshToken(user.UserId, refreshToken, DateTime.Now.AddDays(7).ToUniversalTime());

                Response.Cookies.Append("refreshToken",
                    refreshToken,
                    new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7).ToUniversalTime(),
                    });

                Response.Cookies.Append("accessToken",
                   accessToken,
                   new CookieOptions
                   {
                       Expires = DateTime.UtcNow.AddDays(2).ToUniversalTime(),
                   });

                return RedirectToAction("Index", "Courses");
            }

            ModelState.AddModelError("", "Неправильная электронная почта или пароль!");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout() 
        {
            Response.Cookies.Delete("accessToken");
            return RedirectToAction("Index", "Courses");
        }

        [HttpGet]
        public async Task<IResult> Refresh(
            [FromServices] IUserRepository userRepository,
            [FromServices] IJwtProvider jwtProvider)
        {
            var accessToken = Request.Cookies.ContainsKey("accessToken") ? Request.Cookies["accessToken"] : null;
            var refreshToken = Request.Cookies.ContainsKey("refreshToken") ? Request.Cookies["refreshToken"] : null;

            if (accessToken == null || refreshToken == null)
                return Results.BadRequest("Invalid tokens!");

            var principal = jwtProvider.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Identity?.Name;
            var user = await userRepository.GetByEmail(email);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return Results.BadRequest("Invalid tokens!"); ;

            var newAccessToken = jwtProvider.GenerateAccessToken(user);
            var newRefreshToken = jwtProvider.GenerateRefreshToken();

            await userRepository.SaveRefreshToken(user.UserId, newRefreshToken, DateTime.Now.AddDays(7).ToUniversalTime());
           
            if (newAccessToken != null && newRefreshToken != null)
            {
                Response.Cookies.Append("refreshToken",
                    newRefreshToken,
                    new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                    });

                Response.Cookies.Append("accessToken",
                   newAccessToken,
                   new CookieOptions
                   {
                       Expires = DateTime.UtcNow.AddDays(7),
                   });

                return Results.Ok(new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            return Results.UnprocessableEntity("Invalid refresh token!");   
        }
    }
}