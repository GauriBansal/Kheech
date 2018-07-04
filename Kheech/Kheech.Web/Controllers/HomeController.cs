using Kheech.Web.Clients;
using Kheech.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kheech.Web.Controllers
{
    [RoutePrefix("")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        protected readonly ApplicationDbContext _context;

        protected readonly ApplicationUserManager _userManager;

        public HomeController(ApplicationDbContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("", Name = "IndexPage")]
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("HomePage");
            }
            return View();
        }

        public async Task<ActionResult> Contact()
        {
            return View();
        }
    }
}
