using AutoMapper;
using SubscriptionManager.Entities;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Models.ViewModels;

namespace SubscriptionManager.Profiles;

/// <summary>
/// AutoMapper profile for mapping between entities, DTOs, and view models.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Customer, CustomerViewModel>()
            .ForMember(dest => dest.SubscriptionCount, opt => opt.Ignore());

        // Subscription mappings
        CreateMap<Subscription, SubscriptionDto>().ReverseMap();
        CreateMap<Subscription, SubscriptionViewModel>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : null));
    }
}