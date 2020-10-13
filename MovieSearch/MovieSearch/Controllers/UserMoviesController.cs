using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;

namespace MovieSearch.ViewModels
{
    public class UserMoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<UserMoviesController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserMoviesController(
            ILogger<UserMoviesController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
