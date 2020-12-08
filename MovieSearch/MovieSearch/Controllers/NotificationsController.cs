using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSearch.Data;
using MovieSearch.Models;

namespace MovieSearch.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<NotificationsController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(
            ILogger<NotificationsController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var userNotifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
            return Ok(new { userNotifications, count = userNotifications.Count });
        }

        [HttpPost]
        public async Task<IActionResult> ReadNotification(int id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null || (notification.UserId != _userManager.GetUserId(User))) return NotFound();

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Notification {0} was read", id);

            return Ok();
        }
    }
}
