using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SCORM_Razor.Data.Models;
using SCORM_Razor.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace SCORM_Razor.Pages
{
    public class PackageModel : PageModel
    {
        public SCORMCourse package { get; set; }
        public bool bUserHasCourse { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        private ILogger _logger { get; set; }
        private DatabaseHelper databaseHelper { get; set; }
        private int SCORM_Course_id;
        //Removed Ilogger upload file model
        public PackageModel(UserManager<IdentityUser> User, IConfiguration Configuration, IWebHostEnvironment hostingEnvironment, ILogger<PackageModel> logger)
        {
            _userManager = User;
            _configuration = Configuration;
            _environment = hostingEnvironment;
            _logger = logger;
            databaseHelper = new DatabaseHelper(_logger);
        }
        [Authorize]
        public void OnGet()
        {
            if (!Models.SignedInUser.isSignedIn)
            {
                Response.Redirect("/Identity/Account/Login?returnUrl=" + Request.Path);
            }
            string UserID = _userManager.GetUserId(HttpContext.User);
            if (Request.Query["id"] != String.Empty && UtilityFunctions.isInteger(Request.Query["id"]))
            {
                SCORM_Course_id = Convert.ToInt32(Request.Query["id"]);
                package = databaseHelper.getSCORMCourse(SCORM_Course_id);
                bUserHasCourse = databaseHelper.isCourseInUserStudyArea(SCORM_Course_id, UserID);
            }
        }
        [Authorize]
        public IActionResult OnPost([FromForm] SCORMCourse package)
        {
            string UserID = _userManager.GetUserId(HttpContext.User);
            // determine if course is already in their study area
            // if so, just go to study area
            // if not, add to study area
            if (! databaseHelper.isCourseInUserStudyArea(package.id, UserID))
            {
                // add course to their study area
                databaseHelper.AddCourseToUsersStudyArea(package.id, UserID);
            }
            return RedirectToPage("MyCoursesForStudy");
        }
    }
}