using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly string _path;

        public UserRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException("Path can't be null");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (StreamReader file = new StreamReader(_path))
            {
                User deserializeUser = null;
                string serializeObject = null;
                while (file.Peek() != -1)
                {
                    serializeObject = await file.ReadLineAsync();
                    deserializeUser = JsonConvert.DeserializeObject<User>(serializeObject);
                    users.Add(deserializeUser);
                }
            }

            return users;
        }

        public async Task Add(User user)
        {
            using (StreamWriter file = new StreamWriter(_path, true))
            {
                string serializeUser = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                });

                await file.WriteLineAsync(serializeUser);
            }
        }

        public async Task Delete(User user)
        {
            var allUsers = (await GetAllUsers()).ToList();
            var resultRemoving = allUsers.Remove(user);
            if (!resultRemoving)
            {
                return;
            }

            await WriteUsersToFile(allUsers);
        }

        public async Task Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User can't be null");
            }

            var allUsers = (await GetAllUsers()).ToList();
            bool resultUpdating = false;
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (allUsers[i].IdUser == user.IdUser)
                {
                    allUsers[i] = user;
                    resultUpdating = true;
                }
            }

            if (!resultUpdating)
            {
                return;
            }

            await WriteUsersToFile(allUsers);
        }

        private async Task WriteUsersToFile(List<User> users)
        {
            string serializeUser = null;
            using (StreamWriter file = new StreamWriter(_path))
            {
                for (int i = 0; i < users.Count; i++)
                {
                    serializeUser = JsonConvert.SerializeObject(users[i], new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                    });
                    await file.WriteLineAsync(serializeUser);
                }
            }
        }
    }
}
