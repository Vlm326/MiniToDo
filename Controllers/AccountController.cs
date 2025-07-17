using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MiniToDo.Models;
using MiniToDo.ViewModels;

namespace MiniToDo.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                Console.WriteLine("Login completed");
                return RedirectToAction("Index", "Task");
            }
            else
            {
                ModelState.AddModelError("", "Пользователя не существует или неверные данные");
                Console.WriteLine("Пользователь не создан");
            }
                ModelState.AddModelError("", "Неверный email или пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //ЛОГГИРОВАНИЕ
            Console.WriteLine($"[DEBUG] Модель валидна: {ModelState.IsValid}");
            Console.WriteLine($"[DEBUG] Создаём пользователя: {user.Email}");

            var result = await _userManager.CreateAsync(user, model.Password);
            Console.WriteLine(result);
            foreach (var error in result.Errors)
            {
                Console.WriteLine("Ошибка регистрации: " + error.Description);
                ModelState.AddModelError("", error.Description);
            }

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                Console.WriteLine("User signed in: " + user.Email); // лог
                return RedirectToAction("Index", "Task");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }
    }
}
