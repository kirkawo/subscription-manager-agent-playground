using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SubscriptionManager.Entities;
using SubscriptionManager.Exceptions;
using SubscriptionManager.Interfaces;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Models.ViewModels;

namespace SubscriptionManager.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
    {
        var customers = await _repository.GetAllAsync();

        var viewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customers);

        foreach (var customer in viewModels)
        {
            customer.SubscriptionCount = 0;
        }

        return viewModels;
    }

    public async Task<CustomerViewModel?> GetCustomerByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);

        if (customer == null)
        {
            return null;
        }

        var viewModel = _mapper.Map<CustomerViewModel>(customer);
        viewModel.SubscriptionCount = customer.Subscriptions.Count;

        return viewModel;
    }

    public async Task<CustomerViewModel> CreateCustomerAsync(CustomerDto customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);
        var created = await _repository.CreateAsync(customer);

        var viewModel = _mapper.Map<CustomerViewModel>(created);
        viewModel.SubscriptionCount = 0;

        return viewModel;
    }

    public async Task<CustomerViewModel> UpdateCustomerAsync(int id, CustomerDto customerDto)
    {
        var existingCustomer = await _repository.GetByIdAsync(id);
        if (existingCustomer == null)
        {
            throw new NotFoundException($"Customer with ID {id} not found.");
        }

        _mapper.Map(customerDto, existingCustomer);
        await _repository.UpdateAsync(existingCustomer);

        var viewModel = _mapper.Map<CustomerViewModel>(existingCustomer);
        viewModel.SubscriptionCount = existingCustomer.Subscriptions.Count;

        return viewModel;
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {id} not found.");
        }

        await _repository.DeleteAsync(id);
    }
}