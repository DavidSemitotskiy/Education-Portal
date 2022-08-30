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
            var context = new Context();
            var userRepository = new UserRepository(context);
            var materialRepository = new EntityRepository<Material>(context);
            var courseRepository = new EntityRepository<Course>(context);
            var courseSkillRepository = new EntityRepository<CourseSkill>(context);
            var materialStateRepository = new EntityRepository<MaterialState>(context);
            var courseStateRepository = new EntityRepository<CourseState>(context);
            var userManager = new UserManager(userRepository);
            var materialManager = new MaterialManager(materialRepository);
            var materialStateManager = new MaterialStateManager(materialStateRepository);
            var courseStateManager = new CourseStateManager(courseStateRepository, materialStateManager);
            var courseManager = new CourseManager(courseRepository, courseStateManager);
            var courseSkillManager = new CourseSkillManager(courseSkillRepository);
            var accountController = new AccountController(userManager);
            var materialController = new MaterialController(materialManager);
            var courseSkillController = new CourseSkillController(courseSkillManager);
            var courseController = new CourseController(courseManager, materialController, courseSkillController);
            while (true)
            {
                Console.WriteLine(IUserManager.CurrentUser == null ? "User isn't authorized" : $"Hello {IUserManager.CurrentUser.FirstName} {IUserManager.CurrentUser.LastName}");
                if (IUserManager.CurrentUser == null)
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
                        default:
                            break;
                    }
                }

                Console.Write("Press Enter to continue!!!");
                Console.ReadLine();
                Console.Clear();
            }

            context.Dispose();
        }
    }
}
