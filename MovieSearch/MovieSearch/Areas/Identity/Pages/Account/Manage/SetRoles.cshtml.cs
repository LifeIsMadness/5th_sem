using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieSearch.Models;

namespace MovieSearch.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "SuperAdmin")]
    public class SetRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SetRolesModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public List<RoleViewModel> AllRoles { get; set; }
        //public SetRolesViewModel ViewModel { get; set; }

        public class RoleViewModel
        {
            public string RoleId { get; set; }
            public string RoleName { get; set; }
            public bool Selected { get; set; }
        }

        
        //public class SetRolesViewModel
        //{
        //    public List<RoleViewModel> Roles { get; set; }
        //}

        public async Task LoadAsync(ApplicationUser user)
        {
            var roles = new List<RoleViewModel>();

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                roles.Add(new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Selected = await _userManager.IsInRoleAsync(user, role.Name)
                });

            }

            // ViewModel = new SetRolesViewModel { Roles = roles };
            AllRoles = roles;
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");


            await LoadAsync(user);

            ViewData["userId"] = userId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if(await _userManager.IsInRoleAsync(user, Roles.SuperAdmin.ToString()))
            {
                ModelState.AddModelError("", "Cannot remove role \"superadmin\" ");
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
                ModelState.AddModelError("", "Cannot remove user existing roles");

            result = await _userManager.AddToRolesAsync(user, AllRoles.Where(r => r.Selected).Select(r => r.RoleName));
            if (!result.Succeeded)
                ModelState.AddModelError("", "Cannot add selected roles to user");


            return RedirectToPage("./ManageRoles");
        }
    }
}
