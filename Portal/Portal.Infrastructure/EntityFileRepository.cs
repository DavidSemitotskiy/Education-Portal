using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.Domain.BaseModels;
using Portal.Domain.Interfaces;

namespace Portal.Infrastructure
{
    public class EntityFileRepository<TEntity> : IEntityRepository<TEntity> where TEntity : Entity
    {
        private readonly string _path;

        private readonly JsonSerializerSettings _jsonSettings;

        public EntityFileRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException("Path can't be null");
            _jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public async Task<IEnumerable<TEntity>> GetAllEntities()
        {
            List<TEntity> entities = new List<TEntity>();
            using (StreamReader file = new StreamReader(_path))
            {
                TEntity deserializeEntity = null;
                string serializeObject = null;
                while (file.Peek() != -1)
                {
                    serializeObject = await file.ReadLineAsync();
                    deserializeEntity = JsonConvert.DeserializeObject<TEntity>(serializeObject, _jsonSettings);
                    entities.Add(deserializeEntity);
                }
            }

            return entities;
        }

        public async Task Add(TEntity entity)
        {
            using (StreamWriter file = new StreamWriter(_path, true))
            {
                string serializeEntity = JsonConvert.SerializeObject(entity, _jsonSettings);
                await file.WriteLineAsync(serializeEntity);
            }
        }

        public async Task Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity can't be null");
            }

            var allCourses = (await GetAllEntities()).ToList();
            int indexDeleteEntity = 0;
            bool resultRemoving = false;
            for (int i = 0; i < allCourses.Count; i++)
            {
                if (allCourses[i].Id == entity.Id)
                {
                    indexDeleteEntity = i;
                    resultRemoving = true;
                    break;
                }
            }

            if (!resultRemoving)
            {
                return;
            }

            allCourses.RemoveAt(indexDeleteEntity);
            await WriteEntitiesToFile(allCourses);
        }

        public async Task Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity can't be null");
            }

            var allEntities = (await GetAllEntities()).ToList();
            bool resultUpdating = false;
            for (int i = 0; i < allEntities.Count; i++)
            {
                if (allEntities[i].Id == entity.Id)
                {
                    allEntities[i] = entity;
                    resultUpdating = true;
                    break;
                }
            }

            if (!resultUpdating)
            {
                return;
            }

            await WriteEntitiesToFile(allEntities);
        }

        private async Task WriteEntitiesToFile(List<TEntity> entities)
        {
            string serializeEntity = null;
            using (StreamWriter file = new StreamWriter(_path))
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    serializeEntity = JsonConvert.SerializeObject(entities[i], _jsonSettings);
                    await file.WriteLineAsync(serializeEntity);
                }
            }
        }
    }
}
