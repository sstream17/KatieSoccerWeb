using AutoMapper;
using KatieSoccer.Server.Accessors.EntityFramework.Models;
using KatieSoccer.Shared;

namespace KatieSoccer.Server.Accessors.Mapping
{
    class GameMappingProfile : Profile
    {
        public GameMappingProfile()
        {
            CreateMap<Game, GameData>();
        }
    }
}
