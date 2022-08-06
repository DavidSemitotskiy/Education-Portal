using Portal.Application;
using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Infrastructure;

namespace Portal.ConsoleAPI
{
    public class Program
    {
        public static void Main()
        {
            IUserRepository userRepository = UserRepository.GetInstance();
            IMaterialRepository materialRepository = MaterialRepository.GetInstance();
            ICourseRepository courseRepository = CourseRepository.GetInstance();
            IUserManager userManager = new UserManager(userRepository);
            IMaterialManager materialManager = new MaterialManager(materialRepository);
            ICourseManager courseManager = new CourseManager(courseRepository);
            AccountController accountController = new AccountController(userManager);
            MaterialController materialController = new MaterialController(materialManager);
            CourseController courseController = new CourseController(courseManager, materialController);
            while (true)
            {
                Console.WriteLine(IUserManager.CurrentUser == null ? "User isn't authorized" : $"Hello {IUserManager.CurrentUser.FirstName} {IUserManager.CurrentUser.LastName}");
                Console.WriteLine("1)Register");
                Console.WriteLine("2)Log in");
                Console.WriteLine("3)Log off");
                Console.WriteLine("4)Create course");
                Console.WriteLine("5)Delete course");
                Console.WriteLine("6)Update course");
                Console.WriteLine("7)See available courses");
                Console.Write("Choose the operation by its number: ");
                var pick = Console.ReadLine();
                switch (pick)
                {
                    case "1":
                        accountController.Register();
                        break;
                    case "2":
                        accountController.LogIn();
                        break;
                    case "3":
                        accountController.LogOff();
                        break;
                    case "4":
                        courseController.CreateCourse();
                        break;
                    case "5":
                        courseController.DeleteCourse();
                        break;
                    case "6":
                        courseController.Update();
                        break;
                    case "7":
                        courseController.SeeAvailableCourses();
                        break;
                    default:
                        break;
                }

                Console.Write("Press Enter to continue!!!");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
