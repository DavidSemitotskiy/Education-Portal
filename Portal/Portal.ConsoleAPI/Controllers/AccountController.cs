using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Validation;
using Portal.Domain.DTOs;

namespace Portal.ConsoleAPI.Conrollers
{
    public class AccountController
    {
        public AccountController(IApplicationUserManager userManager)
        {
            UserManager = userManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IApplicationUserManager UserManager { get; }

        public async Task LogIn()
        {
            Console.Write("Input email: ");
            var email = Console.ReadLine();
            Console.Write("Input password: ");
            var password = Console.ReadLine();
            var user = new UserLoginDTO
            {
                Email = email,
                Password = password
            };

            var errorMessages = new ErrorMessages<UserLoginDTOValidator, UserLoginDTO>();
            if (!await errorMessages.Validate(user))
            {
                return;
            }

            await UserManager.ConsoleLogIn(user);
        }

        public async Task Register()
        {
            Console.Write("Input firstname: ");
            var firstName = Console.ReadLine();
            Console.Write("Input lastname: ");
            var lastName = Console.ReadLine();
            Console.Write("Input email: ");
            var email = Console.ReadLine();
            Console.Write("Input password: ");
            var password = Console.ReadLine();
            Console.Write("Confirm password: ");
            var confirmPassword = Console.ReadLine();
            var user = new UserRegisterDTO
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

            await UserManager.ConsoleRegister(user);
            await UserManager.UserRepository.SaveChanges();
        }

        public Task LogOff()
        {
            return UserManager.ConsoleLogOff();
        }

        public async Task SeeUserProfile()
        {
            var currentUserWithIncludes = await UserManager.UserRepository.FindByIdWithIncludesAsync(IApplicationUserManager.CurrentUser.Id, new string[]{ "Skills" });
            Console.WriteLine($"First name: {currentUserWithIncludes.FirstName}");
            Console.WriteLine($"Last name: {currentUserWithIncludes.LastName}");
            Console.WriteLine($"Email: {currentUserWithIncludes.Email}");
            Console.WriteLine($"Access level: {currentUserWithIncludes.AccessLevel}");
            Console.WriteLine("Skills:");
            foreach (var skill in currentUserWithIncludes.Skills)
            {
                Console.WriteLine($"\t- {skill.Experience}  Level: {skill.Level}");
            }
        }
    }
}
