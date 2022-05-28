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
            .ForMember(dest=>dest.RecipeImage, opt => opt.MapFrom(src => src.RecipePath))
            .ForMember(dest => dest.Ingrds, opt => opt.MapFrom(src => src.Ingrds));
        
        CreateMap<StepCreateViewModel,Step>();
        CreateMap<IngredientCreateViewModel,Ingredient>();
        
        CreateMap<Recipe, RecipeSearchViewModel>()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
        CreateMap<Recipe, RecipeDetailsViewModel>()
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps))
            .ForMember(dest => dest.Ingrds, opt => opt.MapFrom(src => src.Ingrds));
        CreateMap<CommentViewModel,Comment >();

    }
}