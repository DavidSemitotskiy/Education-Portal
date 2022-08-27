using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Controllers;
using Portal.ConsoleAPI.Validation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Conrollers
{
    public class CourseController
    {
        public CourseController(ICourseManager courseManager, MaterialController materialController, CourseSkillController courseSkillController)
        {
            CourseManager = courseManager ?? throw new ArgumentNullException("Manager can't be null");
            MaterialController = materialController ?? throw new ArgumentNullException("Controller can't be null");
            CourseSkillController = courseSkillController ?? throw new ArgumentNullException("Controller can't be null");
        }

        public ICourseManager CourseManager { get; set; }

        public MaterialController MaterialController { get; set; }

        public CourseSkillController CourseSkillController { get; set; }

        public async Task CreateCourse()
        {
            string courseName = null;
            string courseDescription = null;
            var accessLevel = 0;
            var countSkills = 0;
            var countMaterials = 0;
            Course course = null;
            while (true)
            {
                Console.Write("Input name of course: ");
                courseName = Console.ReadLine();
                Console.Write("Input description of course: ");
                courseDescription = Console.ReadLine();
                Console.Write("Input access level:");
                var resultParing = int.TryParse(Console.ReadLine(), out accessLevel);
                if (!resultParing || accessLevel < 0)
                {
                    Console.WriteLine("Incorrect access level");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.Write("Input count of skills: ");
                var resultCountSkillsParsing = int.TryParse(Console.ReadLine(), out countSkills);
                if (!resultCountSkillsParsing || countSkills <= 0)
                {
                    Console.WriteLine("Incorrect count!!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.Write("Input count materials: ");
                var resultCountParsing = int.TryParse(Console.ReadLine(), out countMaterials);
                if (!resultCountParsing || countMaterials <= 0)
                {
                    Console.WriteLine("Incorrect count!!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                course = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = courseName,
                    Description = courseDescription,
                    AccessLevel = accessLevel,
                    Skills = new List<CourseSkill>(),
                    Owner = IUserManager.CurrentUser,
                    Materials = new List<Material>()
                };
                var errorMessages = await new ErrorMessages<CourseValidator, Course>().Validate(course);
                if (!errorMessages)
                {
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                break;
            }

            course.Skills = await CourseSkillController.FillCourseSkillsForCourse(countSkills);
            course.Materials = await MaterialController.FillMaterialsForCourse(countMaterials);
            await CourseManager.AddCourse(course);
            await CourseManager.CourseRepository.SaveChanges();
        }

        public async Task SeeAvailableCourses()
        {
            var availableCourses = (await CourseManager.GetAvailableCourses(IUserManager.CurrentUser)).ToList();
            if (availableCourses.Count == 0)
            {
                Console.WriteLine("There aren't any available course by your access level!!!");
                return;
            }

            foreach (var course in availableCourses)
            {
                Console.Write($"{course.Name} - {course.Description} - {course.AccessLevel}");
                Console.WriteLine("\n\tSkills:");
                foreach (var skill in course.Skills)
                {
                    Console.WriteLine($"\t\t-{skill.Experience}");
                }

                Console.WriteLine("\tMaterials:");
                foreach (var material in course.Materials)
                {
                    Console.WriteLine($"\t\t-{material}");
                }
            }
        }

        public async Task DeleteCourse()
        {
            var ownCourses = (await CourseManager.GetOwnCourses(IUserManager.CurrentUser)).ToList();
            if (ownCourses.Count == 0)
            {
                Console.WriteLine("You don't have any courses to delete!!!");
                return;
            }

            for (int i = 0; i < ownCourses.Count; i++)
            {
                Console.Write($"{i + 1}){ownCourses[i].Name} - {ownCourses[i].Description} - {ownCourses[i].AccessLevel}\n");
            }

            Console.Write("Choose own course to delete by its number: ");
            var resultParsing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParsing || index - 1 >= ownCourses.Count)
            {
                Console.WriteLine("Incorrect number of material");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            CourseManager.DeleteCourse(ownCourses[index - 1]);
            await CourseManager.CourseRepository.SaveChanges();
        }

        public async Task Update()
        {
            var ownCourses = (await CourseManager.GetOwnCourses(IUserManager.CurrentUser)).ToList();
            if (ownCourses.Count == 0)
            {
                Console.WriteLine("You don't have any courses to update!!!");
                return;
            }

            for (int i = 0; i < ownCourses.Count; i++)
            {
                Console.Write($"{i + 1}){ownCourses[i].Name} - {ownCourses[i].Description} - {ownCourses[i].AccessLevel}\n");
            }

            Console.Write("Choose own course to update by its number: ");
            var resultParsing = int.TryParse(Console.ReadLine(), out int index);
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
                Console.WriteLine("3)Add skill to list");
                Console.WriteLine("4)Delete skill from list");
                Console.WriteLine("5)Update skill from list");
                Console.WriteLine("6)Add material to course");
                Console.WriteLine("7)Delete material from course");
                Console.WriteLine("8)Update material from course");
                Console.Write("Choose the operation by its number: ");
                var choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        Console.Write("Input new name: ");
                        courseUpdate.Name = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Input new description: ");
                        courseUpdate.Description = Console.ReadLine();
                        break;
                    case "3":
                        courseUpdate.Skills.Add(await CourseSkillController.CreateOrChooseExistedCourseSkill());
                        break;
                    case "4":
                        CourseSkillController.DeleteCourseSkill(courseUpdate);
                        break;
                    case "5":
                        await CourseSkillController.UpdateCourseSkill(courseUpdate);
                        break;
                    case "6":
                        courseUpdate.Materials.Add(await MaterialController.CreateOrChooseExistedMaterial());
                        break;
                    case "7":
                        MaterialController.DeleteMaterial(courseUpdate);
                        break;
                    case "8":
                        await MaterialController.UpdateMaterial(courseUpdate);
                        break;
                    default:
                        Console.WriteLine("Incorrect number of operation");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                }

                CourseManager.UpdateCourse(courseUpdate);
                await CourseManager.CourseRepository.SaveChanges();
                return;
            }
        }
    }
}
