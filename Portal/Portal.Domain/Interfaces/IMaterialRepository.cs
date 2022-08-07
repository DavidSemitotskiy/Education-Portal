namespace Portal.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        List<Material> Materials { get; set; }

        IEnumerable<Material> GetAllMaterials();

        bool Exists(Material material);

        void Add(Material material);

        void Delete(Material material);

        void SaveChanges();
    }
}
