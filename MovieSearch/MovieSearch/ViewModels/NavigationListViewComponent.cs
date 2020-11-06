using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Models;
using MovieSearch.ViewModels.UserMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.ViewModels
{
    public class NavigationListViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public NavigationListViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke(object activePage)
        {
            HttpContext.Request.RouteValues.TryGetValue("userId", out var userId);

            var viewModel = new UserMoviesNavigationViewModel
            {             
                UserId = userId.ToString(),

                ActivePage = activePage.ToString()
            };

            return View(viewModel);
        }
    }

}
