using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly string _path;

        private readonly JsonSerializerSettings _jsonSettings;

        public UserRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException("Path can't be null");
            _jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = new List<User>();
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
                var serializeUser = JsonConvert.SerializeObject(user, _jsonSettings);
                await file.WriteLineAsync(serializeUser);
            }
        }

        public async Task Delete(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User can't be null");
            }

            var allUsers = (await GetAllUsers()).ToList();
            var userToDelete = allUsers.FirstOrDefault(u => u.UserId == user.UserId);
            var resultRemoving = allUsers.Remove(userToDelete);
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
            var resultUpdating = false;
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (allUsers[i].UserId == user.UserId)
                {
                    allUsers[i] = user;
                    resultUpdating = true;
                    break;
                }
            }

            if (!resultUpdating)
            {
                return;
            }

            await WriteUsersToFile(allUsers);
        }

        public async Task SaveChanges()
        {
            await Task.Delay(0);
        }

        private async Task WriteUsersToFile(List<User> users)
        {
            string serializeUser = null;
            using (StreamWriter file = new StreamWriter(_path))
            {
                for (int i = 0; i < users.Count; i++)
                {
                    serializeUser = JsonConvert.SerializeObject(users[i], _jsonSettings);
                    await file.WriteLineAsync(serializeUser);
                }
            }
        }
    }
}
