namespace Portal.WebApp.Models.MaterialViewModels
{
    public class AddBookMaterialViewModel : AddMaterialViewModel
    {
        public string Authors { get; set; }

        public string Title { get; set; }

        public int CountPages { get; set; }

        public string Format { get; set; }

        public DateTime DatePublication { get; set; }
    }
}
