using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddArticleMaterialViewModel : AddMaterialViewModel
    {
        public DateTime DatePublication { get; set; }

        [Required]
        public string? Resource { get; set; }
    }
}
