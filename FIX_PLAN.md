# Fix Plan: SubscriptionManager

## Priority 1 — Build-Breaking Issues

### 1.1 Fix invalid `Microsoft.NET.Test.Sdk` version
- **File:** `tests/SubscriptionManager.Tests/SubscriptionManager.Tests.csproj`
- **Issue:** Version `18.5.1` does not exist on NuGet (latest stable is `17.x`).
- **Fix:** Downgrade to `17.13.0` (latest stable 17.x).

### 1.2 Add project reference from test project to main project
- **File:** `tests/SubscriptionManager.Tests/SubscriptionManager.Tests.csproj`
- **Issue:** Test project cannot reference any types from `SubscriptionManager`.
- **Fix:** Add `<ProjectReference>` to `src/SubscriptionManager/SubscriptionManager.csproj`.

## Priority 2 — Runtime-Breaking Issues

### 2.1 Add database initialization on startup
- **File:** `src/SubscriptionManager/Program.cs`
- **Issue:** No `EnsureCreated()` or migration applied — first query will crash if DB doesn't exist.
- **Fix:** Add `dbContext.Database.EnsureCreated()` call during app startup.

### 2.2 Fix NPE risk in `SubscriptionViewModel.CustomerName` mapping
- **File:** `src/SubscriptionManager/Profiles/MappingProfile.cs`
- **Issue:** `src.Customer.Name` will throw `NullReferenceException` if `Customer` nav property is null.
- **Fix:** Use `opt.MapFrom(src => src.Customer != null ? src.Customer.Name : null)`.

## Priority 3 — Architectural Issues

### 3.1 Make `CustomerService` use `ICustomerRepository` instead of `AppDbContext` directly
- **Files:** `src/SubscriptionManager/Services/CustomerService.cs`
- **Issue:** `CustomerService` bypasses the repository pattern (injects `AppDbContext` directly) while `SubscriptionService` correctly uses the repository. `CustomerRepository` is registered in DI but never used.
- **Fix:** Replace `AppDbContext` dependency with `ICustomerRepository` in `CustomerService`. Move DB query logic to the repository.

### 3.2 Fix inconsistent not-found handling across services
- **Files:** `src/SubscriptionManager/Interfaces/ICustomerService.cs`, `src/SubscriptionManager/Services/CustomerService.cs`
- **Issue:** `ICustomerService.GetCustomerByIdAsync` returns non-nullable `CustomerViewModel` and throws on not-found, while `ISubscriptionService.GetSubscriptionByIdAsync` returns nullable and returns null on not-found.
- **Fix:** Make `ICustomerService.GetCustomerByIdAsync` return `CustomerViewModel?` (nullable) and return null when not found, matching subscription behavior. Remove thrown exception there. The endpoint handler can return a 404 result instead.

### 3.3 Fix inconsistent exception hierarchy
- **Files:** Exception classes in `src/SubscriptionManager/Exceptions/`
- **Issue:** `NotFoundException` (generic), `CustomerNotFoundException`, and `SubscriptionNotFoundException` are all siblings extending `Exception` — no clear hierarchy. `SubscriptionNotFoundException` is unused.
- **Fix:** Make `CustomerNotFoundException` and `SubscriptionNotFoundException` extend `NotFoundException`. Remove `SubscriptionNotFoundException` (or keep and use it consistently). Use `NotFoundException` as a catch-all base.

## Priority 4 — Validation Issues

### 4.1 Register and configure FluentValidation validators
- **Files:** `src/SubscriptionManager/Models/DTOs/CustomerDto.cs`, `src/SubscriptionManager/Models/DTOs/SubscriptionDto.cs`, new validators, `Program.cs`
- **Issue:** `FluentValidation.AspNetCore` is referenced but no validators exist and no validation is performed on request DTOs. Minimal APIs do not automatically validate data annotations on complex body parameters.
- **Fix:** Create `CustomerDtoValidator` and `SubscriptionDtoValidator`, register them, and add validation filter/endpoint filter.

### 4.2 Add `EndDate > StartDate` validation
- **File:** New `SubscriptionDtoValidator`
- **Issue:** No rule ensuring subscription end date is after start date.
- **Fix:** Add `RuleFor(s => s.EndDate).GreaterThan(s => s.StartDate)`.

### 4.3 Add enum for `Subscription.Status` and `Subscription.Currency`
- **File:** `src/SubscriptionManager/Entities/Subscription.cs`
- **Issue:** String fields for status and currency allow invalid values.
- **Fix:** Create enums `SubscriptionStatus` and `CurrencyCode`, use them in entity (with EF Core conversion).

## Priority 5 — Code Quality Issues

### 5.1 Remove redundant dictionary in `GetAllCustomersAsync`
- **File:** `src/SubscriptionManager/Services/CustomerService.cs`
- **Issue:** Unnecessary lookup dictionary when view models map 1:1 with entities.
- **Fix:** Iterate in parallel using `Zip` or just set `SubscriptionCount` during a single pass.

### 5.2 Remove unused imports
- **Files:** `src/SubscriptionManager/Interfaces/ICustomerRepository.cs`, `src/SubscriptionManager/Services/CustomerService.cs`
- **Issue:** Unused `using` directives.
- **Fix:** Remove them.

### 5.3 Remove unused `FluentValidation.AspNetCore` package (or keep if validators added)
- **File:** `src/SubscriptionManager/SubscriptionManager.csproj`
- **Issue:** Package referenced but unused.
- **Fix:** Keep if validators are implemented (see 4.1). Otherwise remove.

### 5.4 Inline DB initialization in Program.cs (remove unused exception handler or keep)

## Priority 6 — Data / Entity Issues

### 6.1 Remove `Id` from `CustomerDto` for create operations
- **File:** `src/SubscriptionManager/Models/DTOs/CustomerDto.cs`
- **Issue:** `POST /customers` allows clients to specify an ID.
- **Fix:** Create separate `CreateCustomerDto` without `Id` and `UpdateCustomerDto` with `Id`, or keep single DTO and ignore `Id` on create.

### 6.2 Add `Phone` and `Address` to `Customer` entity (or remove from DTOs)
- **File:** `src/SubscriptionManager/Entities/Customer.cs`
- **Issue:** `CustomerDto` has `Phone` and `Address` but `Customer` entity doesn't — AutoMapper silently skips them.
- **Fix:** Add `Phone` and `Address` properties to `Customer` entity to match the DTO.

## Priority 7 — Performance Issues

### 7.1 Avoid eager-loading subscriptions in `GET /customers`
- **File:** `src/SubscriptionManager/Services/CustomerService.cs`
- **Issue:** Loading full subscription objects just to count them is wasteful.
- **Fix:** Use a projection query or `Select` count instead of including the full navigation collection.

## Priority 8 — Test Project

### 8.1 Implement meaningful tests
- **File:** `tests/SubscriptionManager.Tests/UnitTest1.cs`
- **Issue:** Test is empty.
- **Fix:** Replace with real unit tests using Moq for the services and repositories.
