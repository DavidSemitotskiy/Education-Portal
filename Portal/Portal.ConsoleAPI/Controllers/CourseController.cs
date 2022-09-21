using Portal.Application.Interfaces;
using Portal.Application.Specifications.CourseSpecifications;
using Portal.Application.Specifications.MaterialSpecifications;
using Portal.Application.Validation;
using Portal.ConsoleAPI.Controllers;
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
                if (!resultCountSkillsParsing || countSkills < 0)
                {
                    Console.WriteLine("Incorrect count!!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.Write("Input count materials: ");
                var resultCountParsing = int.TryParse(Console.ReadLine(), out countMaterials);
                if (!resultCountParsing || countMaterials < 0)
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
                    IsPublished = false,
                    Skills = new List<CourseSkill>(),
                    OwnerUser = IApplicationUserManager.CurrentUser.UserName,
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
            var availableCourses = (await CourseManager.GetAvailableCourses(IApplicationUserManager.CurrentUser)).ToList();
            if (availableCourses.Count == 0)
            {
                Console.WriteLine("There aren't any available course by your access level!!!");
                return;
            }

            await ShowCourses(availableCourses);
        }

        public async Task DeleteCourse()
        {
            var ownCourses = (await CourseManager.GetCoursesNotPublished(IApplicationUserManager.CurrentUser)).ToList();
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
            var ownCourses = (await CourseManager.GetOwnCourses(IApplicationUserManager.CurrentUser)).ToList();
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

            var courseUpdateWithIncludes = await CourseManager.CourseRepository.FindByIdWithIncludesAsync(ownCourses[index - 1].Id, new string[] { "Skills", "Materials" });
            while (true)
            {
                if (courseUpdateWithIncludes.IsPublished)
                {
                    Console.WriteLine("1)Update description");
                }
                else
                {
                    Console.WriteLine("1)Update description");
                    Console.WriteLine("2)Update only name");
                    Console.WriteLine("3)Add skill to course");
                    Console.WriteLine("4)Delete skill from course");
                    Console.WriteLine("5)Update skill from course");
                    Console.WriteLine("6)Add material to course");
                    Console.WriteLine("7)Delete material from course");
                    Console.WriteLine("8)Update material from course");
                    Console.WriteLine("9)Publish course");
                }

                Console.Write("Choose the operation by its number: ");
                var choose = (CourseOperations)int.Parse(Console.ReadLine());
                switch (choose)
                {
                    case CourseOperations.UpdateName:
                        Console.Write("Input new name: ");
                        courseUpdateWithIncludes.Name = Console.ReadLine();
                        break;
                    case CourseOperations.UpdateDescription:
                        Console.Write("Input new description: ");
                        courseUpdateWithIncludes.Description = Console.ReadLine();
                        break;
                    case CourseOperations.AddSkill:
                        var addedSkill = await CourseSkillController.CreateOrChooseExistedCourseSkill();
                        if (courseUpdateWithIncludes.Skills.Contains(addedSkill))
                        {
                            Console.WriteLine("Course already has this skill");
                            Console.Write("Press Enter to continue!!!");
                            Console.ReadLine();
                            Console.Clear();
                            return;
                        }

                        courseUpdateWithIncludes.Skills.Add(addedSkill);
                        break;
                    case CourseOperations.DeleteSkill:
                        CourseSkillController.DeleteCourseSkill(courseUpdateWithIncludes);
                        break;
                    case CourseOperations.UpdateSkill:
                        await CourseSkillController.UpdateCourseSkill(courseUpdateWithIncludes);
                        break;
                    case CourseOperations.AddMaterial:
                        var addedMaterial = await MaterialController.CreateOrChooseExistedMaterial();
                        if (courseUpdateWithIncludes.Materials.Contains(addedMaterial))
                        {
                            Console.WriteLine("Course already has this material");
                            Console.Write("Press Enter to continue!!!");
                            Console.ReadLine();
                            Console.Clear();
                            return;
                        }

                        courseUpdateWithIncludes.Materials.Add(addedMaterial);
                        break;
                    case CourseOperations.DeleteMaterial:
                        MaterialController.DeleteMaterial(courseUpdateWithIncludes);
                        break;
                    case CourseOperations.UpdateMaterial:
                        await MaterialController.UpdateMaterial(courseUpdateWithIncludes);
                        break;
                    case CourseOperations.PublishCourse:
                        if (!CourseManager.PublishCourse(courseUpdateWithIncludes))
                        {
                            throw new ArgumentException("Course must have one or more materials and skills to be published");
                        }

                        break;
                    default:
                        Console.WriteLine("Incorrect number of operation");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                }

                CourseManager.UpdateCourse(courseUpdateWithIncludes);
                await CourseManager.CourseRepository.SaveChanges();
                return;
            }
        }

        public async Task SubscribeCourse()
        {
            var availableCourses = (await CourseManager.GetAvailableCourses(IApplicationUserManager.CurrentUser)).ToList();
            if (availableCourses.Count == 0)
            {
                Console.WriteLine("There aren't any available course by your access level!!!");
                return;
            }

            await ShowCourses(availableCourses);
            Console.Write("Choose the course to subscribe: ");
            var pick = int.Parse(Console.ReadLine()) - 1;
            var courseToSubscribe = availableCourses[pick];
            var courseState = await CourseManager.SubscribeCourse(IApplicationUserManager.CurrentUser, courseToSubscribe);
            await CourseManager.CourseStateManager.CourseStateRepository.SaveChanges();
            if (await CourseManager.CourseStateManager.CheckIfCourseCompleted(IApplicationUserManager.CurrentUser, courseToSubscribe, courseState))
            {
                await CourseManager.CourseStateManager.CourseStateRepository.SaveChanges();
            }
        }

        public async Task SeeCoursesInProgress()
        {
            var allCoursesInProgress = (await CourseManager.GetCoursesInProgress(IApplicationUserManager.CurrentUser)).ToList();
            if (allCoursesInProgress.Count == 0)
            {
                Console.WriteLine("You haven't subscribed on any courses");
                Console.ReadLine();
                return;
            }

            Course course = null;
            for (int i = 0; i < allCoursesInProgress.Count; i++)
            {
                course = await CourseManager.CourseRepository.FindById(allCoursesInProgress[i].CourseId);
                Console.WriteLine($"{i + 1}){course.Name} - {await CourseManager.CourseStateManager.GetCourseProgress(allCoursesInProgress[i])} - {(allCoursesInProgress[i].IsFinished ? "Finished" : "In progress")}");
            }
        }

        public async Task UnSubscribeCourse()
        {
            var userCourseInProgressSpecification = new UserCourseInProgressSpecification(IApplicationUserManager.CurrentUser);
            var notFinishedCourseStateSpecification = new FinishedCourseStateSpecification().Not();
            var finalSpecification = userCourseInProgressSpecification.And(notFinishedCourseStateSpecification);
            var allCoursesInProgress = await CourseManager.CourseStateManager.CourseStateRepository.FindEntitiesBySpecification(finalSpecification);
            if (allCoursesInProgress.Count == 0)
            {
                Console.WriteLine("You haven't subscribed on any courses");
                Console.ReadLine();
                return;
            }

            Course course = null;
            for (int i = 0; i < allCoursesInProgress.Count; i++)
            {
                course = await CourseManager.CourseRepository.FindById(allCoursesInProgress[i].CourseId);
                Console.WriteLine($"{i + 1}){course.Name}");
            }

            Console.Write("Choose the course to subscribe: ");
            var pick = int.Parse(Console.ReadLine()) - 1;
            CourseManager.UnSubscribeCourse(allCoursesInProgress[pick]);
            await CourseManager.CourseStateManager.CourseStateRepository.SaveChanges();
        }

        public async Task CompleteMaterial()
        {
            var userCourseInProgressSpecification = new UserCourseInProgressSpecification(IApplicationUserManager.CurrentUser);
            var notFinishedCourseStateSpecification = new FinishedCourseStateSpecification().Not();
            var finalSpecification = userCourseInProgressSpecification.And(notFinishedCourseStateSpecification);
            var allCoursesInProgress = await CourseManager.CourseStateManager.CourseStateRepository.FindEntitiesBySpecification(finalSpecification);
            if (allCoursesInProgress.Count == 0)
            {
                Console.WriteLine("You don't have any courses in progress");
                Console.ReadLine();
                return;
            }

            Course course = null;
            for (int i = 0; i < allCoursesInProgress.Count; i++)
            {
                course = await CourseManager.CourseRepository.FindById(allCoursesInProgress[i].CourseId);
                Console.WriteLine($"{i + 1}){course.Name}");
            }

            Console.Write("Choose the course to complete material: ");
            var pick = int.Parse(Console.ReadLine()) - 1;
            Material material = null;
            var certainCourseStateWithIncludes = await CourseManager.CourseStateManager.CourseStateRepository
                .FindByIdWithIncludesAsync(allCoursesInProgress[pick].Id, new string[] { "MaterialStates" });
            var notCompleteMaterialStateSpecification = new CompleteMaterialStateSpecification().Not();
            var materialsNotCompleted = certainCourseStateWithIncludes.MaterialStates.Where(notCompleteMaterialStateSpecification.ToExpression().Compile()).ToList();
            for (int i = 0; i < materialsNotCompleted.Count; i++)
            {
                material = await MaterialController.MaterialManager.MaterialRepository
                    .FindById(materialsNotCompleted[i].OwnerMaterial);
                Console.WriteLine($"{i + 1}){material}");
            }

            Console.Write("Choose the material to complete: ");
            var pickMaterial = int.Parse(Console.ReadLine()) - 1;
            CourseManager.CompleteMaterial(materialsNotCompleted[pickMaterial]);
            await CourseManager.CheckIfCoursesCompleted(IApplicationUserManager.CurrentUser, allCoursesInProgress);
            await CourseManager.CourseStateManager.MaterialStateManager.
                MaterialStateRepository.SaveChanges();
        }

        private async Task ShowCourses(List<Course> courses)
        {
            Course courseWithIncludes = null;
            for (int i = 0; i < courses.Count; i++)
            {
                courseWithIncludes = await CourseManager.CourseRepository.FindByIdWithIncludesAsync(courses[i].Id, new string[] { "Skills", "Materials" });
                Console.Write($"{i + 1}){courseWithIncludes.Name} - {courseWithIncludes.Description} - {courseWithIncludes.AccessLevel}");
                Console.WriteLine("\n\tSkills:");
                foreach (var skill in courseWithIncludes.Skills)
                {
                    Console.WriteLine($"\t\t-{skill.Experience}");
                }

                Console.WriteLine("\tMaterials:");
                foreach (var material in courseWithIncludes.Materials)
                {
                    Console.WriteLine($"\t\t-{material}");
                }
            }
        }
    }
}
