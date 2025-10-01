using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Models;
using Tecmave.ViewModels;

namespace Tecmave.Mvc.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<Usuario> signInManager;
        private readonly UserManager<Usuario> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resutl = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (resutl.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario
                {
                    NombreCompleto = model.NombreCompleto,
                    Cedula = model.Cedula,
                    Email = model.Email,
                    UserName = model.Email,

                };

                var result = await userManager.CreateAsync(usuario, model.Password);

                if (result.Succeeded)
                {
                    var roleExists = await roleManager.RoleExistsAsync("User");
                    if (!roleExists)
                    {
                        var role = new IdentityRole("User");
                        await roleManager.CreateAsync(role);
                    }

                    await userManager.AddToRoleAsync(usuario, "User");
                    await signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

            }
            return View(model);
        }


        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Algo salio mal.");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }

            }
            return View(model);
        }



        public IActionResult changePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email no encontrado");
                    return View(model);
                }

            }
            else
            {
                ModelState.AddModelError("", "Algo salio mal. Vuelve a intentar");
                return View(model);
            }

        }

        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
