using INTEX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INTEX.Models.ViewModels;

namespace INTEX.Controllers
{
    [Authorize(Roles = "Admin, Researcher")] // Restrict access to Admins only
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //FOR PASSING THE DATABASE STUFF INTO THE ANALYSIS
        private postgresContext context { get; set; }

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new UserListModel { Users = users, UserRoles = new Dictionary<string, string>() };

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null && roles.Count > 0)
                {
                    model.UserRoles[user.Id] = roles[0];
                }
                else
                {
                    model.UserRoles[user.Id] = "No role assigned";
                }
            }

            return View(model);
        }

        // Add Create, Edit and Delete actions

        public IActionResult Create()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new CreateUserViewModel { Roles = roles };
            return View("CreateUsers", model); // Specify the view name here
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction(nameof(Users));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            var roles = _roleManager.Roles.ToList();
            model.Roles = roles;
            return View(model);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles.ToList();

            // Set the IsEdit property to true and populate the Roles property
            var model = new CreateUserViewModel { UserName = user.UserName, Email = user.Email, IsEdit = true, Roles = roles };

            // Use the "Create" view with the updated model
            return View("CreateUsers", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CreateUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Users));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new CreateUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Users));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        // ADD RECORD PAGE
        [HttpGet]
        [Authorize(Roles = "researcher, admin")]
        public IActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "researcher, admin")]
        public IActionResult AddRecord(Burialmain bm)
        {

            context.Add(bm);
            context.SaveChanges();
            return View("Confirmation", bm);
        }


        [HttpGet]
        [Authorize(Roles = "researcher, admin")]
        public IActionResult Edit(long burialid)
        {
            // Retrieve the burial record with the given burialid from the database
            var burialRecord = context.Burialmain.SingleOrDefault(x => x.Id == burialid);

            if (burialRecord == null)
            {
                return NotFound(); // Return 404 error if the burial record is not found
            }

            // Pass the burial record to the AddRecord view so that it can be edited
            return View("AddRecord", burialRecord);
        }


        [HttpPost]
        [Authorize(Roles = "researcher, admin")]
        public IActionResult Edit(Burialmain bm)
        {
            context.Update(bm);
            context.SaveChanges();

            return RedirectToAction("Data");
        }


        [HttpGet]
        [Authorize(Roles = "researcher, admin")]
        public IActionResult Delete(long burialid)
        {
            var burialRecord = context.Burialmain.SingleOrDefault(x => x.Id == burialid);

            return View(burialRecord);
        }

        [HttpPost]
        [Authorize(Roles = "researcher, admin")]
        public IActionResult Delete(Burialmain bm)
        {
            context.Remove(bm);
            context.SaveChanges();

            return RedirectToAction("Data");
        }
    }
}

