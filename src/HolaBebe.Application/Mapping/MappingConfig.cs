using Mapster;
using HolaBebe.Domain.Entities;
using HolaBebe.Application.Dtos;

namespace HolaBebe.Application.Mapping;

public static class MappingConfig
{
    public static void Register()
    {
        TypeAdapterConfig<UserProfile, UserProfileDto>.NewConfig();
        TypeAdapterConfig<Pregnancy, PregnancyDto>.NewConfig();
    }
}
