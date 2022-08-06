using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Infrastructure
{
    public class CourseRepository : ICourseRepository
    {
        private static CourseRepository _repository = new CourseRepository();

        private CourseRepository()
        {
            Courses = GetAllCourses().ToList();
        }

        public List<Course> Courses { get; set; }

        public static CourseRepository GetInstance()
        {
            return _repository;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            using (StreamReader file = new StreamReader(@"C:\Users\Lementry\OneDrive\Рабочий стол\test\portal\Portal\DB\CourseStorage.json"))
            {
                Course deserializeCourse = null;
                string serializeObject = null;
                while (file.Peek() != -1)
                {
                    serializeObject = file.ReadLine();
                    deserializeCourse = JsonConvert.DeserializeObject<Course>(serializeObject, new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                    yield return deserializeCourse;
                }

                file.Close();
            }
        }

        /// <summary>
        /// Add new Course in the end of file(you don't need to use SaveChanges())
        /// </summary>
        /// <param name="course"></param>
        public void Add(Course course)
        {
            Courses.Add(course);
            using (StreamWriter file = new StreamWriter(@"C:\Users\Lementry\OneDrive\Рабочий стол\test\portal\Portal\DB\CourseStorage.json", true))
            {
                string serializeCourse = JsonConvert.SerializeObject(course, new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    TypeNameHandling = TypeNameHandling.Auto
                });

                file.WriteLine(serializeCourse);
                file.Close();
            }
        }

        /// <summary>
        /// Use SaveChanges to Update file
        /// </summary>
        /// <param name="courseDelete"></param>
        public void Delete(Course courseDelete)
        {
            Courses.Remove(courseDelete);
        }

        public bool Exists(string name, string description)
        {
            return Courses.Any(course => course.Name == name && course.Description == description);
        }

        public void SaveChanges()
        {
            string serializeCourse = null;
            using (StreamWriter file = new StreamWriter(@"C:\Users\Lementry\OneDrive\Рабочий стол\test\portal\Portal\DB\CourseStorage.json"))
            {
                for (int i = 0; i < Courses.Count; i++)
                {
                    serializeCourse = JsonConvert.SerializeObject(Courses[i], new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    file.WriteLine(serializeCourse);
                }

                file.Close();
            }
        }
    }
}
