using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Validation;
using Portal.Domain.DTOs;

namespace Portal.ConsoleAPI.Conrollers
{
    public class AccountController
    {
        public AccountController(IUserManager userManager)
        {
            UserManager = userManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IUserManager UserManager { get; }

        public async Task LogIn()
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

            var errorMessages = new ErrorMessages<UserLoginDTOValidator, UserLoginDTO>();
            if (!await errorMessages.Validate(user))
            {
                return;
            }

            await UserManager.LogIn(user);
        }

        public async Task Register()
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
            var errorMessages = new ErrorMessages<UserRegisterDTOValidator, UserRegisterDTO>();
            if (!await errorMessages.Validate(user))
            {
                return;
            }

            await UserManager.Register(user);
        }

        public async Task LogOff()
        {
            await UserManager.LogOff();
        }
    }
}
