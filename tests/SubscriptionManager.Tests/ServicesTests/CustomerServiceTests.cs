using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Application.Exceptions;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Application.Services;
using SubscriptionManager.Application.Models.DTOs;
using SubscriptionManager.Application.Profiles;

namespace SubscriptionManager.Tests.ServicesTests;

public class CustomerServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _mapper, NullLogger<CustomerService>.Instance);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ReturnsAllCustomers()
    {
        var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Alice" },
                new Customer { Id = 2, Name = "Bob" }
            };
        _customerRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        var result = await _customerService.GetAllCustomersAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsNull_WhenNotFound()
    {
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var result = await _customerService.GetCustomerByIdAsync(42);

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
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(customer);

        var result = await _customerService.GetCustomerByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Alice", result.Name);
        Assert.Equal(1, result.SubscriptionCount);
    }

    [Fact]
    public async Task DeleteCustomerAsync_Throws_WhenNotFound()
    {
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _customerService.DeleteCustomerAsync(99));
    }

    [Fact]
    public async Task CreateCustomerAsync_ReturnsCreatedViewModel()
    {
        var dto = new CustomerDto { Name = "Bob", Email = "bob@test.com" };
        var created = new Customer { Id = 1, Name = "Bob", Email = "bob@test.com" };
        _customerRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(created);

        var result = await _customerService.CreateCustomerAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Bob", result.Name);
    }

    [Fact]
    public async Task UpdateCustomerAsync_Throws_WhenNotFound()
    {
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _customerService.UpdateCustomerAsync(1, new CustomerDto { Name = "New", Email = "new@test.com" }));
    }

    [Fact]
    public async Task UpdateCustomerAsync_ReturnsUpdatedViewModel_WhenFound()
    {
        var existing = new Customer { Id = 1, Name = "OldName", Email = "old@test.com" };
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        var dto = new CustomerDto { Name = "NewName", Email = "new@test.com" };

        var result = await _customerService.UpdateCustomerAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("NewName", result.Name);
    }

    [Fact]
    public async Task DeleteCustomerAsync_CallsDelete_WhenFound()
    {
        var existing = new Customer { Id = 1, Name = "Alice", Email = "alice@test.com" };
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _customerService.DeleteCustomerAsync(1);

        _customerRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
