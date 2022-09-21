using System.ComponentModel.DataAnnotations;

namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddBookMaterialViewModel : AddMaterialViewModel
    {
        [Required]
        public string Authors { get; set; }

        [Required]
        public string Title { get; set; }

        public int CountPages { get; set; }

        [Required]
        public string Format { get; set; }

        public DateTime DatePublication { get; set; }
    }
}
