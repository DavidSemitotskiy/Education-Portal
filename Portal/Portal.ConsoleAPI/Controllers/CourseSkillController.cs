using Portal.Application.Interfaces;
using Portal.Application.Validation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Controllers
{
    public class CourseSkillController
    {
        public CourseSkillController(ICourseSkillManager manager)
        {
            CourseSkillManager = manager ?? throw new ArgumentNullException("Managaer can't be null");
        }

        public ICourseSkillManager CourseSkillManager { get; }

        public Task<CourseSkill> CreateOrChooseExistedCourseSkill()
        {
            while (true)
            {
                Console.WriteLine("1)Create skill");
                Console.WriteLine("2)Choose existed skill");
                Console.Write("Choose the operation by its number: ");
                var pick = Console.ReadLine();
                switch (pick)
                {
                    case "1":
                        return CreateCourseSkill();
                    case "2":
                        return ChooseExistedCourseSkill();
                }

                Console.WriteLine("Incorrect number of operation");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public async Task<List<CourseSkill>> FillCourseSkillsForCourse(int countCourseSkills)
        {
            CourseSkill skill = null;
            var skills = new List<CourseSkill>();
            for (int i = 0; i < countCourseSkills;)
            {
                skill = await CreateOrChooseExistedCourseSkill();
                if (skills.Contains(skill))
                {
                    Console.WriteLine("Course already has this skill");
                    Console.Write("Press Enter to continue!!!");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                skills.Add(skill);
                i++;
                Console.Write("Press Enter to continue!!!");
                Console.ReadLine();
                Console.Clear();
            }

            return skills;
        }

        public async Task<CourseSkill> CreateCourseSkill()
        {
            while (true)
            {
                Console.Write("Input name of skill: ");
                string strSkill = Console.ReadLine();
                var courseSkill = new CourseSkill
                {
                    Id = Guid.NewGuid(),
                    Experience = strSkill,
                    Courses = new List<Course>()
                };
                var resultValidation = await new ErrorMessages<CourseSkillValidator, CourseSkill>().Validate(courseSkill);
                if (!resultValidation)
                {
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                var createdSkill = await CourseSkillManager.CreateOrGetExistedCourseSkill(courseSkill);
                await CourseSkillManager.CourseSkillRepository.SaveChanges();
                return createdSkill;
            }
        }

        public async Task<CourseSkill> ChooseExistedCourseSkill()
        {
            var allCourseSkills = (await CourseSkillManager.CourseSkillRepository.GetAllEntities()).ToList();
            if (allCourseSkills.Count == 0)
            {
                Console.WriteLine("There aren't any skills");
                return await CreateCourseSkill();
            }

            while (true)
            {
                for (int i = 0; i < allCourseSkills.Count; i++)
                {
                    Console.WriteLine($"{i + 1}){allCourseSkills[i].Experience}");
                }

                Console.Write("Choose one by its number: ");
                var resultParing = int.TryParse(Console.ReadLine(), out int index);
                if (!resultParing || index - 1 >= allCourseSkills.Count)
                {
                    Console.WriteLine("Incorrect number of existed skills");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                return allCourseSkills[index - 1];
            }
        }

        public void DeleteCourseSkill(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't b null");
            }

            if (course.Skills.Count == 0)
            {
                Console.WriteLine("There aren't any skills to delete");
                return;
            }

            for (int i = 0; i < course.Skills.Count; i++)
            {
                Console.WriteLine($"{i + 1}){course.Skills[i].Experience}");
            }

            Console.Write("Choose the skill to delete by its number: ");
            var resultParing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParing || index - 1 >= course.Skills.Count)
            {
                Console.WriteLine("Incorrect number of skill");
                return;
            }

            CourseSkillManager.DeleteCourseSkill(course, course.Skills[index - 1]);
        }

        public async Task UpdateCourseSkill(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't b null");
            }

            if (course.Skills.Count == 0)
            {
                Console.WriteLine("There aren't any skills to update");
                return;
            }

            for (int i = 0; i < course.Skills.Count; i++)
            {
                Console.WriteLine($"{i + 1}){course.Skills[i].Experience}");
            }

            Console.Write("Choose the skill to update by its number: ");
            var resultParing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParing || index - 1 >= course.Skills.Count)
            {
                Console.WriteLine("Incorrect number of material");
                return;
            }

            var updatedSkill = await CreateOrChooseExistedCourseSkill();
            if (course.Skills.Contains(updatedSkill))
            {
                Console.WriteLine("Course already has this skill");
                Console.Write("Press Enter to continue!!!");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            CourseSkillManager.UpdateCourseSkill(course, index - 1, updatedSkill);
        }
    }
}
