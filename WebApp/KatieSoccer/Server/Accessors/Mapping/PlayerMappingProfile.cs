using AutoMapper;

namespace KatieSoccer.Server.Accessors.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Shared.Player, EntityFramework.Models.Player>()
                .ReverseMap();
        }
    }
}
