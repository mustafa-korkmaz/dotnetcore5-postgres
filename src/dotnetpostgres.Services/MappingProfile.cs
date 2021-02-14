
using AutoMapper;

namespace dotnetpostgres.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullCollections = true;

            // Add as many of these lines as you need to map your objects
            CreateMap<Dto.DtoBase, Dal.Entities.EntityBase>();
            CreateMap<Dal.Entities.EntityBase, Dto.DtoBase>();

            CreateMap<Dto.Customer.Customer, Dal.Entities.Customer>();
            CreateMap<Dal.Entities.Customer, Dto.Customer.Customer>();

            CreateMap<Dal.Entities.Identity.ApplicationUser, Dto.ApplicationUser>()
                .ForMember(dest => dest.RawSettings,
                    opt => opt.MapFrom(src => src.Settings));

            CreateMap<Dto.ApplicationUser, Dal.Entities.Identity.ApplicationUser>()
                .ForMember(dest => dest.Settings,
                    opt => opt.MapFrom(src => src.RawSettings));
        }
    }
}