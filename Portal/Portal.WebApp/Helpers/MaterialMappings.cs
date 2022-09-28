using AutoMapper;
using Portal.Domain.Models;
using Portal.WebApp.Models.MaterialViewModels;

namespace Portal.WebApp.Helpers
{
    public class MaterialMappings : Profile
    {
        public MaterialMappings()
        {
            CreateMap<AddBookMaterialViewModel, BookMaterial>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            });
        }
    }
}
