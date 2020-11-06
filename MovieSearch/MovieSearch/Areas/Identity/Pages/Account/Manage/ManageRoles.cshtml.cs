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
    public class ManageRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageRolesModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // [BindProperty]
        public UsersWithRolesViewModel ViewModel { get; set; }

        public class UserRolesViewModel
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Roles { get; set; }
        }

        public class UsersWithRolesViewModel
        {
            public List<UserRolesViewModel> Users {get;set;}
        }

        private async Task LoadAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<UserRolesViewModel>();

            foreach (ApplicationUser user in users)
            {
                var viewModel = new UserRolesViewModel();
                viewModel.UserId = user.Id;
                viewModel.UserName = user.UserName;
                viewModel.Email = user.Email;
                viewModel.Roles = string.Join(", ", await _userManager.GetRolesAsync(user));
                usersWithRoles.Add(viewModel);
            }

            ViewModel = new UsersWithRolesViewModel { Users = usersWithRoles };
        }

        //TODO: Managing roles on superadmin's account
        public async Task<IActionResult> OnGet()
        {
            await LoadAsync();
            return Page();
        }
    }
}

