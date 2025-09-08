using CleanTemplate.Application.Domain.Implementation;
using CleanTemplate.Application.DTO;
using AutoMapper;

namespace CleanTemplate.Application.Mapping
{
    public class CleanTemplateApplicationMapping : Profile
    {
        public CleanTemplateApplicationMapping()
        {
            // CleanEntity mappings
            CreateMap<CleanEntityPayLoadDTO, CleanEntity>();
            CreateMap<CleanEntity, CleanEntityPayLoadDTO>();
            CreateMap<CleanEntity, CleanEntityResponseDTO>();
            CreateMap<CleanEntityResponseDTO, CleanEntity>();
        }
    }
}