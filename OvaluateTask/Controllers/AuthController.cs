using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OvaluateTask.Models;
using OvaluateTask.Services;
using System.Threading.Tasks;

namespace OvaluateTask.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> logger;
        private readonly IAuthManager authManager;
        public AuthController(IAuthManager authManager, ILogger<AuthController> logger)
        {
            this.authManager = authManager;
            this.logger = logger;
        }
        // GET: AuthController
        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult Register()
        {
            return View("Register");
        }

        // POST: AuthController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterNewUser(RegisterModel model)
        {
            logger.LogInformation("Register New User End-Point Called");
            try
            {
                var response = await authManager.RegisterUserAsync(model);
                if (response.IsSuccess)
                {
                    ViewBag.message = "User Created Successfully";
                    return View("Login");

                }
                ViewBag.message = response.Message;
                return View("Register");
            }
            catch
            {
                return View("Register");
            }
        }
        // POST: AuthController/LoginUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginUser(LoginModel model)
        {
            logger.LogInformation("Login User End-Point Called");
            try
            {
                var response = await authManager.LoginUser(model);
                if (response.IsSuccess)
                {
                  return RedirectToAction("Index", "Customer");

                }
                ViewBag.message = response.Message;
                return View("Login");
            }
            catch
            {
                return View("Login");
            }
        }


    }
}
