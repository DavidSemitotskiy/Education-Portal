namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddArticleMaterialViewModel : AddMaterialViewModel
    {
        public DateTime DatePublication { get; set; }

        public string? Resource { get; set; }
    }
}
