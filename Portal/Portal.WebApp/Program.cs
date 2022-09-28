using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Portal.Application;
using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.EFInfrastructure;
using Portal.EFInfrastructure.Repositories;
using Portal.WebApp;
using Portal.WebApp.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PaginationOptions>(builder.Configuration.GetSection("PaginationOptions"));

builder.Services.AddDbContext<Context>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<Context>();

builder.Services.AddScoped<IEntityRepository<Course>, EntityRepository<Course>>();
builder.Services.AddScoped<IEntityRepository<CourseSkill>, EntityRepository<CourseSkill>>();
builder.Services.AddScoped<IEntityRepository<CourseState>, EntityRepository<CourseState>>();
builder.Services.AddScoped<IEntityRepository<Material>, EntityRepository<Material>>();
builder.Services.AddScoped<IEntityRepository<MaterialState>, EntityRepository<MaterialState>>();
builder.Services.AddScoped<IEntityRepository<CourseSkill>, EntityRepository<CourseSkill>>();
builder.Services.AddScoped<IEntityRepository<UserSkill>, EntityRepository<UserSkill>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IMaterialStateManager, MaterialStateManager>();
builder.Services.AddScoped<IUserSkillManager, UserSkillManager>();
builder.Services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
builder.Services.AddScoped<ICourseStateManager, CourseStateManager>();
builder.Services.AddScoped<ICourseManager, CourseManager>();
builder.Services.AddScoped<ICourseSkillManager, CourseSkillManager>();
builder.Services.AddScoped<IMaterialManager, MaterialManager>();

builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions
{
    ProgressBar = true,
    TimeOut = 5000
});

builder.Services.AddCloudscribePagination();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new UserMappings());
    config.AddProfile(new CourseMappings());
},typeof(Program).Assembly);

var app = builder.Build();

app.UseNToastNotify();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
