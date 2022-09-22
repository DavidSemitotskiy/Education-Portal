using Portal.Domain.Models;

namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddExistingMaterialViewModel
    {
        public Guid IdCourse { get; set; }

        public List<Material> Materials { get; set; }
    }
}
