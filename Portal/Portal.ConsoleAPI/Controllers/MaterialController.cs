using Portal.Application.Interfaces;
using Portal.ConsoleAPI.Validation;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI.Conrollers
{
    public class MaterialController
    {
        public MaterialController(IMaterialManager materialManager)
        {
            MaterialManager = materialManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IMaterialManager MaterialManager { get; set; }

        public Task<Material> CreateOrChooseExistedMaterial()
        {
            while (true)
            {
                Console.WriteLine("1)Create material");
                Console.WriteLine("2)Choose existed material");
                Console.Write("Choose the operation by its number: ");
                var pick = Console.ReadLine();
                switch (pick)
                {
                    case "1":
                        return CreatOwnMaterial();
                    case "2":
                        return ChooseExistedMaterial();
                }

                Console.WriteLine("Incorrect number of operation");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public async Task<List<Material>> FillMaterialsForCourse(int countMaterials)
        {
            Material material = null;
            var materials = new List<Material>();
            for (int i = 0; i < countMaterials;)
            {
                material = await CreateOrChooseExistedMaterial();
                if (materials.Contains(material))
                {
                    Console.WriteLine("Course already has this material");
                    Console.Write("Press Enter to continue!!!");
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

            return materials;
        }

        public void DeleteMaterial(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't b null");
            }

            if (course.Materials.Count == 0)
            {
                Console.WriteLine("There aren't any materials to delete");
                return;
            }

            for (int i = 0; i < course.Materials.Count; i++)
            {
                Console.WriteLine($"{i + 1}){course.Materials[i]}");
            }

            Console.Write("Choose the material to delete by its number: ");
            var resultParing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParing || index - 1 >= course.Materials.Count)
            {
                Console.WriteLine("Incorrect number of material");
                return;
            }

            MaterialManager.DeleteMaterial(course, course.Materials[index - 1]);
        }

        public async Task UpdateMaterial(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't b null");
            }

            if (course.Materials.Count == 0)
            {
                Console.WriteLine("There aren't any materials to update");
                return;
            }

            for (int i = 0; i < course.Materials.Count; i++)
            {
                Console.WriteLine($"{i + 1}){course.Materials[i]}");
            }

            Console.Write("Choose the material to update by its number: ");
            var resultParing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParing || index - 1 >= course.Materials.Count)
            {
                Console.WriteLine("Incorrect number of material");
                return;
            }

            MaterialManager.UpdateMaterial(course, index - 1, await CreateOrChooseExistedMaterial());
        }

        public Task<Material> CreatOwnMaterial()
        {
            while (true)
            {
                Console.WriteLine("1)Create BookMaterial");
                Console.WriteLine("2)Create VidoeMaterial");
                Console.WriteLine("3)Create ArticleMaterial");
                Console.Write("Choose the operation by its number: ");
                var pick = Console.ReadLine();
                switch (pick)
                {
                    case "1":
                        return CreateBookMaterial();
                    case "2":
                        return CreateVidoeMaterial();
                    case "3":
                        return CreateArticleMaterial();
                    default:
                        Console.WriteLine("Incorrect number of operation");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }
        }

        public async Task<Material> CreateBookMaterial()
        {
            while (true)
            {
                Console.Write("Input authors: ");
                var authors = Console.ReadLine();
                Console.Write("Input title: ");
                var title = Console.ReadLine();
                Console.Write("Input count pages: ");
                var resultParing = int.TryParse(Console.ReadLine(), out int countPages);
                if (!resultParing || countPages <= 0)
                {
                    Console.WriteLine("Incorrect count pages");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.Write("Input format: ");
                var format = Console.ReadLine();
                Console.Write("Input publication date: ");
                var resultParingDate = DateTime.TryParse(Console.ReadLine(), out DateTime datePublication);
                if (!resultParingDate)
                {
                    Console.WriteLine("Incorrect date");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                var material = new BookMaterial
                {
                    Id = Guid.NewGuid(),
                    Authors = authors,
                    Title = title,
                    CountPages = countPages,
                    Format = format,
                    DatePublication = datePublication
                };
                var errorMessages = await new ErrorMessages<BookMaterialValidator, BookMaterial>().Validate(material);
                if (!errorMessages)
                {
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                return await MaterialManager.CreateOrGetExistedMaterial(material);
            }
        }

        public async Task<Material> CreateVidoeMaterial()
        {
            while (true)
            {
                Console.Write("Input duration: ");
                var resultParing = long.TryParse(Console.ReadLine(), out long duration);
                if (!resultParing || duration < 0)
                {
                    Console.WriteLine("Incorrect duration");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.Write("Input quality: ");
                var quality = Console.ReadLine();
                var material = new VideoMaterial
                {
                    Id = Guid.NewGuid(),
                    Duration = duration,
                    Quality = quality
                };
                var errorMessages = await new ErrorMessages<VideoMaterialValidator, VideoMaterial>().Validate(material);
                if (!errorMessages)
                {
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                return await MaterialManager.CreateOrGetExistedMaterial(material);
            }
        }

        public async Task<Material> CreateArticleMaterial()
        {
            while (true)
            {
                Console.Write("Input source: ");
                var resource = Console.ReadLine();
                Console.Write("Input publication date: ");
                var resultParingDate = DateTime.TryParse(Console.ReadLine(), out DateTime datePublication);
                if (!resultParingDate)
                {
                    Console.WriteLine("Incorrect date");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                var material = new ArticleMaterial
                {
                    Id = Guid.NewGuid(),
                    Resource = resource,
                    DatePublication = datePublication
                };
                var errorMessages = await new ErrorMessages<ArticleMaterialValidator, ArticleMaterial>().Validate(material);
                if (!errorMessages)
                {
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                return await MaterialManager.CreateOrGetExistedMaterial(material);
            }
        }

        public async Task<Material> ChooseExistedMaterial()
        {
            var allMaterials = (await MaterialManager.MaterialRepository.GetAllEntities()).ToList();
            if (allMaterials.Count == 0)
            {
                Console.WriteLine("There aren't any materials");
                return await CreatOwnMaterial();
            }

            while (true)
            {
                for (int i = 0; i < allMaterials.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {allMaterials[i]}");
                }

                Console.Write("Choose one by its number: ");
                var resultParing = int.TryParse(Console.ReadLine(), out int index);
                if (!resultParing || index - 1 >= allMaterials.Count)
                {
                    Console.WriteLine("Incorrect number of existed material");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                return allMaterials[index - 1];
            }
        }
    }
}
