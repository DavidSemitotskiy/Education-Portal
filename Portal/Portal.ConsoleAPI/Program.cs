using Portal.Application;
using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Conrollers;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Infrastructure;

namespace Portal.ConsoleAPI
{
    public class Program
    {
        public static async Task Main()
        {
            IUserRepository userRepository = new UserRepository(@"..\..\..\..\DataBases\UserStorage.json");
            IEntityRepository<Material> materialRepository = new EntityFileRepository<Material>(@"..\..\..\..\DataBases\MaterialStorage.json");
            IEntityRepository<Course> courseRepository = new EntityFileRepository<Course>(@"..\..\..\..\DataBases\CourseStorage.json");
            IUserManager userManager = new UserManager(userRepository);
            IMaterialManager materialManager = new MaterialManager(materialRepository);
            ICourseManager courseManager = new CourseManager(courseRepository);
            AccountController accountController = new AccountController(userManager);
            MaterialController materialController = new MaterialController(materialManager);
            CourseController courseController = new CourseController(courseManager, materialController);
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
                    int offset = 2;
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
        }
    }
}
