using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCORM_Razor.Data.Models;
using SCORM_Razor.Helpers;
using System.Collections.Generic;

namespace SCORM_Razor.Pages
{
    public class PackageListingModel : PageModel
    {
        public List<SCORM_Course_fromSP> listPackagesWithUser { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        private ILogger _logger { get; set; }
        private DatabaseHelper databaseHelper { get; set; }
        //Removed Ilogger uploadfilemodel
        public PackageListingModel(UserManager<IdentityUser> User, IConfiguration Configuration, IWebHostEnvironment hostingEnvironment, ILogger<PackageListingModel> logger)
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
            listPackagesWithUser = databaseHelper.getCourseListWithUserIndicator(UserID);
        }
    }
}