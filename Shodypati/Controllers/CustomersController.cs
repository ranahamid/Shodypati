using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shodypati.Models;

namespace Shodypati.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : BaseController
    {
        // GET: Customers
        public async Task<ActionResult> Index()
        {
            const string role = "Customer";
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
                if (await UserManager.IsInRoleAsync(user.Id, role))
                    users.Add(user);
            return View(users);
        }
    }
}