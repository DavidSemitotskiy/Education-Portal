using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddVideoMaterialViewModel : AddMaterialViewModel
    {
        public long Duration { get; set; }

        [Required]
        public string? Quality { get; set; }
    }
}
