using AutoMapper;
using Prodemos.Application.Dtos.Championship;
using Prodemos.Application.Dtos.Matches;
using Prodemos.Application.Dtos.Team;
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
    }
}
