using AutoMapper;
using Prodemos.Application.Dtos.Championship;
using Prodemos.Application.Dtos.Matches;
using Prodemos.Application.Dtos.Team;
using Prodemos.Application.Dtos.UserGuests;
using Prodemos.Application.Dtos.UserPlays;
using Prodemos.Domain;

namespace Prodemos.Application;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Team, TeamResponseDto>();
        CreateMap<Championship, ChampionshipResponseDto>();
        CreateMap<Match, ChampionshipMatchDto>();
        CreateMap<Match, MatchResponseDto>();
        CreateMap<UserPlay, UserPlayResponseDto>();
        CreateMap<UserGuest, UserGuestUserPlayDto>().ForMember(x => x.TeamAName, opt => opt.MapFrom(src => src.Match!.TeamA!.Name))
                                                    .ForMember(x => x.TeamBName, opt => opt.MapFrom(src => src.Match!.TeamB!.Name));
        CreateMap<UserGuest, UserGuestResponseDto>().ForMember(x => x.TeamAName, opt => opt.MapFrom(src => src.Match!.TeamA!.Name))
                                                   .ForMember(x => x.TeamBName, opt => opt.MapFrom(src => src.Match!.TeamB!.Name));
    }
}
