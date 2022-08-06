using Portal.Application.Interfaces;
using Portal.Domain.DTOs;

namespace Portal.ConsoleAPI
{
    public class AccountController
    {
        public AccountController(IUserManager userManager)
        {
            UserManager = userManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IUserManager UserManager { get; set; }

        public void LogIn()
        {
            Console.Write("Input email: ");
            string email = Console.ReadLine();
            Console.Write("Input password: ");
            string password = Console.ReadLine();
            UserLoginDTO user = new UserLoginDTO
            {
                Email = email,
                Password = password
            };
            UserManager.LogIn(user);
        }

        public void Register()
        {
            Console.Write("Input firstname: ");
            string firstName = Console.ReadLine();
            Console.Write("Input lastname: ");
            string lastName = Console.ReadLine();
            Console.Write("Input email: ");
            string email = Console.ReadLine();
            Console.Write("Input password: ");
            string password = Console.ReadLine();
            Console.Write("Confirm password: ");
            string confirmPassword = Console.ReadLine();
            UserRegisterDTO user = new UserRegisterDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            UserManager.Register(user);
        }

        public void LogOff()
        {
            UserManager.LogOff();
        }
    }
}
