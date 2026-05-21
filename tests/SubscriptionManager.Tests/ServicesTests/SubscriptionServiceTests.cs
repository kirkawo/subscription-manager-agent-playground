using AutoMapper;
using Moq;
using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Application.Exceptions;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Application.Models.DTOs;
using SubscriptionManager.Application.Profiles;
using SubscriptionManager.Application.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace SubscriptionManager.Tests.ServicesTests;

public class SubscriptionServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ISubscriptionRepository> _repositoryMock;
    private readonly SubscriptionService _service;

    public SubscriptionServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _repositoryMock = new Mock<ISubscriptionRepository>();
        _service = new SubscriptionService(_repositoryMock.Object, _mapper, NullLogger<SubscriptionService>.Instance);
    }

    [Fact]
    public async Task GetSubscriptionByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Subscription?)null);

        var result = await _service.GetSubscriptionByIdAsync(42);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSubscriptionByIdAsync_ReturnsViewModel_WhenFound()
    {
        var subscription = new Subscription
        {
            Id = 1,
            Name = "Test",
            CustomerId = 1,
            Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@test.com" }
        };
        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(subscription);

        var result = await _service.GetSubscriptionByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
        Assert.Equal("Test Customer", result.CustomerName);
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_Throws_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Subscription?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteSubscriptionAsync(99));
    }

    [Fact]
    public async Task GetAllSubscriptionsAsync_ReturnsAllSubscriptions()
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription { Id = 1, Name = "Basic" },
            new Subscription { Id = 2, Name = "Premium" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(subscriptions);

        var result = await _service.GetAllSubscriptionsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ReturnsCreatedViewModel()
    {
        var dto = new SubscriptionDto { Name = "Basic Plan" };
        var created = new Subscription
        {
            Id = 1,
            Name = "Basic Plan",
            CustomerId = 1,
            Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@test.com" }
        };
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Subscription>())).ReturnsAsync(created);

        var result = await _service.CreateSubscriptionAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Basic Plan", result.Name);
    }

    [Fact]
    public async Task UpdateSubscriptionAsync_Throws_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Subscription?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateSubscriptionAsync(1, new SubscriptionDto { Name = "Updated" }));
    }

    [Fact]
    public async Task UpdateSubscriptionAsync_ReturnsUpdatedViewModel_WhenFound()
    {
        var existing = new Subscription { Id = 1, Name = "OldName" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        var dto = new SubscriptionDto { Name = "NewName" };

        var result = await _service.UpdateSubscriptionAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("NewName", result.Name);
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_CallsDelete_WhenFound()
    {
        var existing = new Subscription { Id = 1, Name = "ToDelete" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _service.DeleteSubscriptionAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
