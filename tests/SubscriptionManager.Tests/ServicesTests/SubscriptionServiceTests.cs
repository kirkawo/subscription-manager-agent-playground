using AutoMapper;
using Moq;
using SubscriptionManager.Entities;
using SubscriptionManager.Exceptions;
using SubscriptionManager.Interfaces;
using SubscriptionManager.Profiles;
using SubscriptionManager.Services;
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
}
