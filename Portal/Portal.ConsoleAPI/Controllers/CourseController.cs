using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Validation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Conrollers
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

        public async Task CreateCourse()
        {
            string courseName = null;
            string courseDescription = null;
            var accessLevel = 0;
            List<CourseSkill> courseSkills = null;
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

                Console.Write("Input skills of course: ");
                var skills = Console.ReadLine();
                var listSkills = skills.Split(',');
                courseSkills = listSkills.Select(strSkill => new CourseSkill
                {
                    Id = Guid.NewGuid(),
                    Experience = strSkill
                }).ToList();
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
                    Skills = courseSkills,
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

            Material material = null;
            var materials = new List<Material>();
            for (int i = 0; i < countMaterials;)
            {
                Console.WriteLine("1)Create own material");
                Console.WriteLine("2)Choose existed material");
                Console.Write("Choose the operation by its number: ");
                var pick = Console.ReadLine();
                switch (pick)
                {
                    case "1":
                        material = await MaterialController.CreatOwnMaterial();
                        break;
                    case "2":
                        material = await MaterialController.ChooseExistedMaterial();
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

            course.Materials = materials;
            await CourseManager.AddCourse(course);
            await CourseManager.CourseRepository.SaveChanges();
            return;
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
                Console.Write($"{course.Name} - {course.Description} - {course.AccessLevel}\n");
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

            await CourseManager.DeleteCourse(ownCourses[index - 1]);
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
                Console.WriteLine("3)Update material");
                Console.Write("Choose the operation by its number: ");
                var choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        Console.Write("Input new name: ");
                        courseUpdate.Name = Console.ReadLine();
                        await CourseManager.UpdateCourse(courseUpdate);
                        await CourseManager.CourseRepository.SaveChanges();
                        return;
                    case "2":
                        Console.Write("Input new description: ");
                        courseUpdate.Description = Console.ReadLine();
                        await CourseManager.UpdateCourse(courseUpdate);
                        await CourseManager.CourseRepository.SaveChanges();
                        return;
                    case "3":
                        await MaterialController.UpdateMaterial(courseUpdate);
                        await CourseManager.UpdateCourse(courseUpdate);
                        await CourseManager.CourseRepository.SaveChanges();
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
