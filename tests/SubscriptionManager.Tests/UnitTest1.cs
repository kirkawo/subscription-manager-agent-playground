using AutoMapper;
using Moq;
using SubscriptionManager.Entities;
using SubscriptionManager.Exceptions;
using SubscriptionManager.Interfaces;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Models.ViewModels;
using SubscriptionManager.Profiles;
using SubscriptionManager.Services;

namespace SubscriptionManager.Tests;

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
        _service = new SubscriptionService(_repositoryMock.Object, _mapper);
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

public class CustomerServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _repositoryMock = new Mock<ICustomerRepository>();
        _service = new CustomerService(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var result = await _service.GetCustomerByIdAsync(42);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsViewModelWithCount_WhenFound()
    {
        var customer = new Customer
        {
            Id = 1,
            Name = "Alice",
            Email = "alice@test.com",
            Subscriptions = new List<Subscription>
            {
                new() { Id = 1, Name = "Sub1" }
            }
        };
        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(customer);

        var result = await _service.GetCustomerByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Alice", result.Name);
        Assert.Equal(1, result.SubscriptionCount);
    }

    [Fact]
    public async Task DeleteCustomerAsync_Throws_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteCustomerAsync(99));
    }
}
