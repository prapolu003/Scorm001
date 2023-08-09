using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using SCORM_Razor.Data.Models;
using Microsoft.AspNetCore.Builder;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using SCORM_Razor.Helpers;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();



 builder.Services.Configure<CookiePolicyOptions>(options =>
 {
     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
     options.CheckConsentNeeded = context => true;
     options.MinimumSameSitePolicy = SameSiteMode.None;
 });

// builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//configuration.SetBasePath(Directory.GetCurrentDirectory());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpContextAccessor();  

builder.Services.AddControllers()
    .AddNewtonsoftJson();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}



//var Configuration = app.Services.GetRequiredService<IConfiguration> ();
app.UseHtmlHandler();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

//var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
ConfigurationHelper.Initialize(configuration, httpContextAccessor);
// we run SCORM courses out of its own folder (NOT wwwroot) so that HtmlHandler can check for authentication before returning any SCORM content
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine( builder.Environment.ContentRootPath , ConfigurationHelper.CourseFolder)),
    RequestPath = "/SCORMCourses"
});

app.Run();
