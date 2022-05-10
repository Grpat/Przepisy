using AutoMapper;
using Hostele.Models;
using Hostele.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hostele;

public class HostelProfile:Profile
{
    public HostelProfile()
    {
        CreateMap<RecipeCreateViewModel, Recipe>()
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps))
            .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.RecipeIngredients));
        CreateMap<StepCreateViewModel,Step>();
        CreateMap<RecipeIngredientCreateViewModel,RecipeIngredient>();
        
        CreateMap<Recipe, RecipeSearchViewModel>();
        CreateMap<Recipe, RecipeDetailsViewModel>()
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps))
            .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.RecipeIngredients));
        

    }
}