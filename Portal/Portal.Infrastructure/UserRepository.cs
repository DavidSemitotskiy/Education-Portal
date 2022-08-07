using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private static UserRepository _repository = new UserRepository();

        private UserRepository()
        {
            Users = GetAllUsers().ToList();
        }

        public List<User> Users { get; set; }

        public static UserRepository GetInstance()
        {
            return _repository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (StreamReader file = new StreamReader(@"..\..\..\..\DataBases\UserStorage.json"))
            {
                User deserializeUser = null;
                string serializeObject = null;
                while (file.Peek() != -1)
                {
                    serializeObject = file.ReadLine();
                    deserializeUser = JsonConvert.DeserializeObject<User>(serializeObject);
                    yield return deserializeUser;
                }

                file.Close();
            }
        }

        public void Add(User user)
        {
            Users.Add(user);
            using (StreamWriter file = new StreamWriter(@"..\..\..\..\DataBases\UserStorage.json", true))
            {
                string serializeUser = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

                file.WriteLine(serializeUser);
                file.Close();
            }
        }

        public bool Exists(string firstName, string lastName, string email)
        {
            return Users.Any(user => (user.FirstName == firstName && user.LastName == lastName) || user.Email == email);
        }

        public User GetLogInUser(UserLoginDTO userLogIn)
        {
            if (userLogIn == null)
            {
                throw new ArgumentNullException("LogIn User can't be null");
            }

            foreach (var user in Users)
            {
                if (user.Email == userLogIn.Email && user.Password == userLogIn.Password)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
