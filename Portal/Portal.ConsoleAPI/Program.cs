﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portal.Application;
using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Conrollers;
using Portal.ConsoleAPI.Controllers;
using Portal.Domain.Models;
using Portal.EFInfrastructure;
using Portal.EFInfrastructure.Repositories;

namespace Portal.ConsoleAPI
{
    public class Program
    {
        public static async Task Main()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            var config = configBuilder.Build();
            var connectionString = config.GetConnectionString("DatabaseConnection");
            var efOptions = new DbContextOptionsBuilder<Context>();
            efOptions.UseSqlServer(connectionString);
            var context = new Context(efOptions.Options);
            var userRepository = new UserRepository(context);
            var materialRepository = new EntityRepository<Material>(context);
            var courseRepository = new EntityRepository<Course>(context);
            var courseSkillRepository = new EntityRepository<CourseSkill>(context);
            var materialStateRepository = new EntityRepository<MaterialState>(context);
            var courseStateRepository = new EntityRepository<CourseState>(context);
            var userSkillRepository = new EntityRepository<UserSkill>(context);
            var userSkillManager = new UserSkillManager(userSkillRepository);
            var applicationUserManager = new ApplicationUserManager(userRepository);
            var materialManager = new MaterialManager(materialRepository);
            var materialStateManager = new MaterialStateManager(materialStateRepository);
            var courseStateManager = new CourseStateManager(courseStateRepository, materialStateManager, userSkillManager, applicationUserManager);
            var courseManager = new CourseManager(courseRepository, courseStateManager);
            var courseSkillManager = new CourseSkillManager(courseSkillRepository);
            var accountController = new AccountController(applicationUserManager);
            var materialController = new MaterialController(materialManager);
            var courseSkillController = new CourseSkillController(courseSkillManager);
            var courseController = new CourseController(courseManager, materialController, courseSkillController);
            while (true)
            {
                try
                {
                    Console.WriteLine(IApplicationUserManager.CurrentUser == null ? "User isn't authorized" : $"Hello {IApplicationUserManager.CurrentUser.FirstName} {IApplicationUserManager.CurrentUser.LastName}");
                    if (IApplicationUserManager.CurrentUser == null)
                    {
                        Console.WriteLine("1)Register");
                        Console.WriteLine("2)Log in");
                        Console.Write("Choose the operation by its number: ");
                        var pick = (Operations)int.Parse(Console.ReadLine());
                        switch (pick)
                        {
                            case Operations.Register:
                                await accountController.Register();
                                break;
                            case Operations.LogIn:
                                await accountController.LogIn();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("1)Log off");
                        Console.WriteLine("2)Create course");
                        Console.WriteLine("3)Delete course");
                        Console.WriteLine("4)Update course");
                        Console.WriteLine("5)See available courses");
                        Console.WriteLine("6)Subscribe on course");
                        Console.WriteLine("7)See courses in progress");
                        Console.WriteLine("8)Unsubscribe course");
                        Console.WriteLine("9)Complete material");
                        Console.WriteLine("10)See user profile");
                        var offset = 2;
                        Console.Write("Choose the operation by its number: ");
                        var pick = (Operations)(int.Parse(Console.ReadLine()) + offset);
                        switch (pick)
                        {
                            case Operations.LogOff:
                                await accountController.LogOff();
                                break;
                            case Operations.CreateCourse:
                                await courseController.CreateCourse();
                                break;
                            case Operations.DeleteCourse:
                                await courseController.DeleteCourse();
                                break;
                            case Operations.Update:
                                await courseController.Update();
                                break;
                            case Operations.SeeAvailableCourses:
                                await courseController.SeeAvailableCourses();
                                break;
                            case Operations.SubscribeCourse:
                                await courseController.SubscribeCourse();
                                break;
                            case Operations.SeeCoursesInProgress:
                                await courseController.SeeCoursesInProgress();
                                break;
                            case Operations.UnSubscribeCourse:
                                await courseController.UnSubscribeCourse();
                                break;
                            case Operations.CompleteMaterial:
                                await courseController.CompleteMaterial();
                                break;
                            case Operations.SeeProfile:
                                await accountController.SeeUserProfile();
                                break;
                            default:
                                break;
                        }
                    }

                    Console.Write("Press Enter to continue!!!");
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Write("Press Enter to continue!!!");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            context.Dispose();
        }
    }
}
