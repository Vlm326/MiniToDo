using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MiniToDo.Models;
using MiniToDo.ViewModels;

namespace MiniToDo.Controllers
{
    public class AccountActionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountActionsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Manage()
        {
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("[ERROR] Пользователь не найден.");
                ModelState.AddModelError("", "Пользователь не найден");
                return View("Index");
            }

            var email = user.Email;

            await _signInManager.SignOutAsync();
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                Console.WriteLine($"[DEBUG] Пользователь {email} успешно удалён.");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Console.WriteLine($"[ERROR] Ошибка при удалении пользователя {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                ModelState.AddModelError("", "Ошибка при удалении пользователя");
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("[ERROR] Пользователь не найден.");
                ModelState.AddModelError("", "Пользователь не найден");
                return View("Index");
            }

            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isOldPasswordValid)
            {
                Console.WriteLine("[ERROR] Старый пароль неверный.");
                ModelState.AddModelError("", "Старый пароль неверный");
                return View("Index");
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                Console.WriteLine("[DEBUG] Пароль успешно изменён.");
                return RedirectToAction("Index", "Task");
            }
            else
            {
                Console.WriteLine($"[ERROR] Ошибка при изменении пароля: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                ModelState.AddModelError("", "Ошибка при изменении пароля");
                return View("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Console.WriteLine("[DEBUG] Пользователь вышел из системы.");
            return RedirectToAction("Index", "Home");
        }
    }
}
