using Mapster;
using HolaBebe.Domain.Entities;
using HolaBebe.Application.Dtos;

namespace HolaBebe.Application.Mapping;

public static class MappingConfig
{
    public static void Register()
    {
        TypeAdapterConfig<UserProfile, UserProfileDto>.NewConfig();
        TypeAdapterConfig<UserProfileDto, UserProfile>.NewConfig();

        TypeAdapterConfig<Pregnancy, PregnancyDto>.NewConfig();
        TypeAdapterConfig<PregnancyCreateDto, Pregnancy>.NewConfig();

        TypeAdapterConfig<FruitSizeCatalog, FruitSizeDto>.NewConfig();

        TypeAdapterConfig<CalendarEvent, CalendarEventDto>.NewConfig();
        TypeAdapterConfig<CalendarEventCreateDto, CalendarEvent>.NewConfig();

        TypeAdapterConfig<Article, ArticleListDto>.NewConfig();
        TypeAdapterConfig<Article, ArticleDto>.NewConfig();

        TypeAdapterConfig<TutorialSlide, TutorialSlideDto>.NewConfig();
        TypeAdapterConfig<WeeklyContent, WeeklyContentDto>.NewConfig();
    }
}
