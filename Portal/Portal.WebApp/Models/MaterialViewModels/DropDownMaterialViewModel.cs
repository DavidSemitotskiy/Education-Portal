using Portal.Domain.Models;

namespace Portal.WebApp.Models.MaterialViewModels
{
    public class DropDownMaterialViewModel
    {
        public Guid IdCourse { get; set; }

        public Guid SelectedMaterialId { get; set; }

        public List<Material> Materials { get; set; } = new List<Material>();
    }
}
