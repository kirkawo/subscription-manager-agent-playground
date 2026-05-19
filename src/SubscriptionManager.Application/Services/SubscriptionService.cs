using AutoMapper;
using Microsoft.Extensions.Logging;
using SubscriptionManager.Application.Exceptions;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Application.Models.DTOs;
using SubscriptionManager.Application.Models.ViewModels;
using SubscriptionManager.Domain.Entities;

namespace SubscriptionManager.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(ISubscriptionRepository repository, IMapper mapper, ILogger<SubscriptionService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SubscriptionViewModel>> GetAllSubscriptionsAsync()
    {
        _logger?.LogInformation("Fetching all subscriptions");
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SubscriptionViewModel>>(entities);
    }

    public async Task<SubscriptionViewModel?> GetSubscriptionByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            _logger?.LogWarning("Subscription with ID {Id} not found", id);
            return null;
        }
        return _mapper.Map<SubscriptionViewModel>(entity);
    }

    public async Task<SubscriptionViewModel> CreateSubscriptionAsync(SubscriptionDto dto)
    {
        var entity = _mapper.Map<Subscription>(dto);
        var created = await _repository.CreateAsync(entity);
        _logger?.LogInformation("Creating new subscription with ID {Id}", created.Id);
        return _mapper.Map<SubscriptionViewModel>(created);
    }

    public async Task<SubscriptionViewModel> UpdateSubscriptionAsync(int id, SubscriptionDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new NotFoundException($"Subscription with ID {id} not found.");
        }
        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
        _logger?.LogInformation("Subscription {Id} updated successfully", id);
        return _mapper.Map<SubscriptionViewModel>(existing);
    }

    public async Task DeleteSubscriptionAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new NotFoundException($"Subscription with ID {id} not found.");
        }
        _logger?.LogInformation("Subscription {Id} deleted", id);
        await _repository.DeleteAsync(id);
    }
}
