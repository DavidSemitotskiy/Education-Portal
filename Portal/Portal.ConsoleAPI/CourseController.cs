using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ConsoleAPI
{
    public class CourseController
    {
        public CourseController(ICourseManager courseManager, MaterialController materialController)
        {
            CourseManager = courseManager ?? throw new ArgumentNullException("Manager can't be null");
            MaterialController = materialController ?? throw new ArgumentNullException("Controller can't be null");
        }

        public ICourseManager CourseManager { get; set; }

        public MaterialController MaterialController { get; set; }

        public void CreateCourse()
        {
            string courseName = null;
            string courseDescription = null;
            int accessLevel = 0;
            List<Skill> courseSkills = null;
            int countMaterials = 0;
            while (true)
            {
                Console.Write("Input name of course: ");
                courseName = Console.ReadLine();
                Console.Write("Input description of course: ");
                courseDescription = Console.ReadLine();
                Console.Write("Input access level:");
                bool resultParing = int.TryParse(Console.ReadLine(), out accessLevel);
                if (!resultParing || accessLevel < 0)
                {
                    Console.WriteLine("Incorrect access level");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.Write("Input skills of course: ");
                string skills = Console.ReadLine();
                var listSkills = skills.Split(',');
                courseSkills = listSkills.Select(strSkill => new Skill
                {
                    Experience = strSkill
                }).ToList();
                Console.Write("Input count materials: ");
                bool resultCountParsing = int.TryParse(Console.ReadLine(), out countMaterials);
                if (!resultCountParsing || countMaterials <= 0)
                {
                    Console.WriteLine("Incorrect count!!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
            }

            Material material = null;
            List<Material> materials = new List<Material>();
            for (int i = 0; i < countMaterials;)
            {
                Console.WriteLine("1)Create own material");
                Console.WriteLine("2)Choose existed material");
                Console.Write("Choose the operation by its number: ");
                string pick = Console.ReadLine();
                switch (pick)
                {
                    case "1":
                        material = MaterialController.CreatOwnMaterial();
                        break;
                    case "2":
                        material = MaterialController.ChooseExistedMaterial();
                        break;
                    default:
                        Console.WriteLine("Incorrect number of operation");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                }

                materials.Add(material);
                i++;
                Console.Write("Press Enter to continue!!!");
                Console.ReadLine();
                Console.Clear();
            }

            Course course = new Course
            {
                IdCourse = Guid.NewGuid(),
                Name = courseName,
                Description = courseDescription,
                AccessLevel = accessLevel,
                Skills = courseSkills,
                Owner = IUserManager.CurrentUser,
                Subscribers = new List<User>(),
                Materials = materials
            };
            CourseManager.AddCourse(course);
        }

        public void SeeAvailableCourses()
        {
            var availableCourses = CourseManager.GetAvailableCourses(IUserManager.CurrentUser).ToList();
            if (availableCourses.Count == 0)
            {
                Console.WriteLine("There aren't any available course by your access level!!!");
                return;
            }

            foreach (var course in availableCourses)
            {
                Console.Write($"{course.Name} - {course.Description} - {course.AccessLevel}");
            }
        }

        public void DeleteCourse()
        {
            var ownCourses = CourseManager.GetOwnCourses(IUserManager.CurrentUser).ToList();
            if (ownCourses.Count == 0)
            {
                Console.WriteLine("You don't have any courses to delete!!!");
                return;
            }

            for (int i = 0; i < ownCourses.Count; i++)
            {
                Console.Write($"{i + 1}){ownCourses[i].Name} - {ownCourses[i].Description} - {ownCourses[i].AccessLevel}");
            }

            Console.Write("Choose own course to delete by its number: ");
            bool resultParsing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParsing || index - 1 >= ownCourses.Count)
            {
                Console.WriteLine("Incorrect number of material");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            CourseManager.DeleteCourse(ownCourses[index - 1]);
        }

        public void Update()
        {
            var ownCourses = CourseManager.GetOwnCourses(IUserManager.CurrentUser).ToList();
            if (ownCourses.Count == 0)
            {
                Console.WriteLine("You don't have any courses to update!!!");
                return;
            }

            for (int i = 0; i < ownCourses.Count; i++)
            {
                Console.Write($"{i + 1}){ownCourses[i].Name} - {ownCourses[i].Description} - {ownCourses[i].AccessLevel}");
            }

            Console.Write("Choose own course to update by its number: ");
            bool resultParsing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParsing || index - 1 >= ownCourses.Count)
            {
                Console.WriteLine("Incorrect number of material");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            var courseUpdate = ownCourses[index - 1];
            while (true)
            {
                Console.WriteLine("1)Update only name");
                Console.WriteLine("2)Update description");
                Console.WriteLine("3)Update material");
                Console.Write("Choose the operation by its number: ");
                string choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        courseUpdate.Name = Console.ReadLine();
                        return;
                    case "2":
                        courseUpdate.Description = Console.ReadLine();
                        return;
                    case "3":
                        MaterialController.UpdateMaterial(courseUpdate);
                        return;
                    default:
                        Console.WriteLine("Incorrect number of operation");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
