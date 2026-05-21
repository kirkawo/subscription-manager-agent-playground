using AutoMapper;
using SubscriptionManager.Application.Models.DTOs;
using SubscriptionManager.Application.Models.ViewModels;
using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Customer, CustomerViewModel>()
            .ForMember(dest => dest.SubscriptionCount, opt => opt.Ignore());

        CreateMap<Subscription, SubscriptionDto>().ReverseMap();
        CreateMap<Subscription, SubscriptionViewModel>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : null));
    }
}
