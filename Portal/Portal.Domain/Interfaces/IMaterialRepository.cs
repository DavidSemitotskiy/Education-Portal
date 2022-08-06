namespace Portal.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        IEnumerable<Material> GetAllMaterials();

        void Add(Material material);

        void Delete(Material material);

        void SaveChanges();
    }
}
