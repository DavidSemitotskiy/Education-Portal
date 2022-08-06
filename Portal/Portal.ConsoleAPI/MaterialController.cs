using Portal.Application.Interfaces;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;

namespace Portal.ConsoleAPI
{
    public class MaterialController
    {
        public MaterialController(IMaterialManager materialManager)
        {
            MaterialManager = materialManager ?? throw new ArgumentNullException("Manager can't be null");
        }

        public IMaterialManager MaterialManager { get; set; }

        public void UpdateMaterial(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course can't b null");
            }

            for (int i = 0; i < course.Materials.Count; i++)
            {
                Console.WriteLine($"{i + 1}){course.Materials[i]}");
            }

            Console.Write("Choose the material to update by its number: ");
            bool resultParing = int.TryParse(Console.ReadLine(), out int index);
            if (!resultParing || index - 1 >= course.Materials.Count)
            {
                Console.WriteLine("Incorrect number of material");
                return;
            }

            course.Materials[index - 1] = CreatOwnMaterial();
        }

        public Material CreatOwnMaterial()
        {
            while (true)
            {
                Console.WriteLine("1)Create BookMaterial");
                Console.WriteLine("2)Create VidoeMaterial");
                Console.WriteLine("3)Create ArticleMaterial");
                Console.Write("Choose the operation by its number: ");
                string pick = Console.ReadLine();
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

        public Material CreateBookMaterial()
        {
            while (true)
            {
                Console.Write("Input authors: ");
                string authors = Console.ReadLine();
                Console.Write("Input title: ");
                string title = Console.ReadLine();
                Console.Write("Input count pages: ");
                bool resultParing = int.TryParse(Console.ReadLine(), out int countPages);
                if (!resultParing || countPages <= 0)
                {
                    Console.WriteLine("Incorrect count pages");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                string format = Console.ReadLine();
                bool resultParingDate = DateTime.TryParse(Console.ReadLine(), out DateTime datePublication);
                if (!resultParingDate)
                {
                    Console.WriteLine("Incorrect date");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                BookMaterial material = new BookMaterial
                {
                    IdMaterial = Guid.NewGuid(),
                    Authors = authors,
                    Title = title,
                    CountPages = countPages,
                    Format = format
                };

                MaterialManager.AddMaterial(material);
                return material;
            }
        }

        public Material CreateVidoeMaterial()
        {
            while (true)
            {
                Console.Write("Input duration: ");
                bool resultParing = long.TryParse(Console.ReadLine(), out long duration);
                if (!resultParing || duration < 0)
                {
                    Console.WriteLine("Incorrect duration");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                string quality = Console.ReadLine();
                VideoMaterial material = new VideoMaterial
                {
                    IdMaterial = Guid.NewGuid(),
                    Duration = duration,
                    Quality = quality
                };
                MaterialManager.AddMaterial(material);
                return material;
            }
        }

        public Material CreateArticleMaterial()
        {
            while (true)
            {
                string resource = Console.ReadLine();
                bool resultParingDate = DateTime.TryParse(Console.ReadLine(), out DateTime datePublication);
                if (!resultParingDate)
                {
                    Console.WriteLine("Incorrect date");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                ArticleMaterial material = new ArticleMaterial
                {
                    IdMaterial = Guid.NewGuid(),
                    Resource = resource,
                    DatePublication = datePublication
                };
                MaterialManager.AddMaterial(material);
                return material;
            }
        }

        public Material ChooseExistedMaterial()
        {
            var allMaterials = MaterialManager.MaterialRepository.GetAllMaterials().ToList();
            if (allMaterials.Count == 0)
            {
                Console.WriteLine("There aren't any materials");
                return CreatOwnMaterial();
            }

            while (true)
            {
                for (int i = 0; i < allMaterials.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {allMaterials[i]}");
                }

                Console.Write("Choose one by its number: ");
                bool resultParing = int.TryParse(Console.ReadLine(), out int index);
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
