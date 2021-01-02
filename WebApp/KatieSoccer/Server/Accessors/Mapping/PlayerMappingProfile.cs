using System;
using AutoMapper;

namespace KatieSoccer.Server.Accessors.Mapping
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Shared.Player, EntityFramework.Models.Player>()
                .ForMember(
                    p => p.IsLocal,
                    options => options.MapFrom(model => Convert.ToInt32(model.IsLocal)));

            CreateMap<EntityFramework.Models.Player, Shared.Player>()
                .ForMember(
                    model => model.IsLocal,
                    options => options.MapFrom(p => Convert.ToBoolean(p.IsLocal)));
        }
    }
}
