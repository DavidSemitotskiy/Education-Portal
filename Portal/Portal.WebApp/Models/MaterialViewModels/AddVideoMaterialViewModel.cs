namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddVideoMaterialViewModel : AddMaterialViewModel
    {
        public long Duration { get; set; }

        public string? Quality { get; set; }
    }
}
