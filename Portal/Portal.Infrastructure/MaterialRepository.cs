using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Infrastructure
{
    internal class MaterialRepository : IMaterialRepository
    {
        private static MaterialRepository _repository = new MaterialRepository();

        private MaterialRepository()
        {
            Materials = GetAllMaterials().ToList();
        }

        public List<Material> Materials { get; set; }

        public static MaterialRepository GetInstance()
        {
            return _repository;
        }

        public IEnumerable<Material> GetAllMaterials()
        {
            using (StreamReader file = new StreamReader(@"C:\Users\Lementry\OneDrive\Рабочий стол\test\portal\Portal\DB\MaterialStorage.json"))
            {
                Material deserializeMaterial = null;
                string serializeObject = null;
                while (file.Peek() != -1)
                {
                    serializeObject = file.ReadLine();
                    deserializeMaterial = JsonConvert.DeserializeObject<Material>(serializeObject, new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                    yield return deserializeMaterial;
                }

                file.Close();
            }
        }

        /// <summary>
        /// Add new Material in the end of file(you don't need to use SaveChanges())
        /// </summary>
        /// <param name="material"></param>
        public void Add(Material material)
        {
            Materials.Add(material);
            using (StreamWriter file = new StreamWriter(@"C:\Users\Lementry\OneDrive\Рабочий стол\test\portal\Portal\DB\MaterialStorage.json", true))
            {
                string serializeMaterial = JsonConvert.SerializeObject(material, new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    TypeNameHandling = TypeNameHandling.All
                });
                file.WriteLine(serializeMaterial);
                file.Close();
            }
        }

        /// <summary>
        /// Use SaveChanges to Update file
        /// </summary>
        /// <param name="material"></param>
        public void Delete(Material material)
        {
            Materials.Remove(material);
        }

        public void SaveChanges()
        {
            string serializeMaterial = null;
            using (StreamWriter file = new StreamWriter(@"C:\Users\Lementry\OneDrive\Рабочий стол\test\portal\Portal\DB\CourseStorage.json"))
            {
                for (int i = 0; i < Materials.Count; i++)
                {
                    serializeMaterial = JsonConvert.SerializeObject(Materials[i], new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    file.WriteLine(serializeMaterial);
                }

                file.Close();
            }
        }
    }
}
