using AutoMapper;

namespace APICatalogo.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProdutoDTO, ProdutoDTO>().ReverseMap();
            CreateMap<CategoriaDTO, CategoriaDTO>().ReverseMap();
        }
    }
}
