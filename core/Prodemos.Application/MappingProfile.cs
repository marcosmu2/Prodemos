using AutoMapper;
using Prodemos.Application.Dtos.Team;
using Prodemos.Domain;

namespace Prodemos.Application;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Team, TeamResponseDto>();
    }
}
