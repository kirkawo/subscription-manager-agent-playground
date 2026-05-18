using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SubscriptionManager.Entities;
using SubscriptionManager.Exceptions;
using SubscriptionManager.Interfaces;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Models.ViewModels;

namespace SubscriptionManager.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _repository;
    private readonly IMapper _mapper;

    public SubscriptionService(ISubscriptionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubscriptionViewModel>> GetAllSubscriptionsAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SubscriptionViewModel>>(entities);
    }

    public async Task<SubscriptionViewModel?> GetSubscriptionByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;
        return _mapper.Map<SubscriptionViewModel>(entity);
    }

    public async Task<SubscriptionViewModel> CreateSubscriptionAsync(SubscriptionDto dto)
    {
        var entity = _mapper.Map<Subscription>(dto);
        var created = await _repository.CreateAsync(entity);
        return _mapper.Map<SubscriptionViewModel>(created);
    }

    public async Task<SubscriptionViewModel> UpdateSubscriptionAsync(int id, SubscriptionDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException($"Subscription with ID {id} not found.");
        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
        return _mapper.Map<SubscriptionViewModel>(existing);
    }

    public async Task DeleteSubscriptionAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException($"Subscription with ID {id} not found.");
        await _repository.DeleteAsync(id);
    }
}
