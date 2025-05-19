using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Controllers
{
    public class SalesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public SalesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateSale_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            var request = new CreateSaleRequest
            {
                SaleNumber = "SALE-001",
                CustomerId = Guid.NewGuid(),
                CustomerName = "John Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 1",
                Items = new[]
                {
                    new CreateSaleRequest.SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 1",
                        Quantity = 5,
                        UnitPrice = 100m
                    }
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/sales", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<CreateSaleResponse>();
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        [Fact]
        public async Task CreateSale_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateSaleRequest
            {
                SaleNumber = "",
                CustomerId = Guid.Empty,
                CustomerName = "",
                BranchId = Guid.Empty,
                BranchName = "",
                Items = Array.Empty<CreateSaleRequest.SaleItemRequest>()
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/sales", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetSaleById_WithExistingSale_ShouldReturnSale()
        {
            // Arrange
            var createRequest = new CreateSaleRequest
            {
                SaleNumber = "SALE-002",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Jane Doe",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 2",
                Items = new[]
                {
                    new CreateSaleRequest.SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 2",
                        Quantity = 3,
                        UnitPrice = 200m
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);
            var createResult = await createResponse.Content.ReadFromJsonAsync<CreateSaleResponse>();

            // Act
            var response = await _client.GetAsync($"/api/sales/{createResult.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UpdateSaleResponse>();
            Assert.NotNull(result);
            Assert.Equal(createRequest.SaleNumber, result.SaleNumber);
        }

        [Fact]
        public async Task GetSaleById_WithNonExistingSale_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.GetAsync($"/api/sales/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateSale_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var createRequest = new CreateSaleRequest
            {
                SaleNumber = "SALE-003",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Bob Smith",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 3",
                Items = new[]
                {
                    new CreateSaleRequest.SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 3",
                        Quantity = 2,
                        UnitPrice = 300m
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);
            var createResult = await createResponse.Content.ReadFromJsonAsync<CreateSaleResponse>();

            var updateRequest = new UpdateSaleRequest
            {
                Id = createResult.Id,
                Items = new[]
                {
                    new UpdateSaleRequest.SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 4",
                        Quantity = 4,
                        UnitPrice = 150m
                    }
                }
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/sales/{createResult.Id}", updateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CancelSale_WithExistingSale_ShouldReturnOk()
        {
            // Arrange
            var createRequest = new CreateSaleRequest
            {
                SaleNumber = "SALE-004",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Alice Johnson",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 4",
                Items = new[]
                {
                    new CreateSaleRequest.SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Product 5",
                        Quantity = 1,
                        UnitPrice = 500m
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);
            var createResult = await createResponse.Content.ReadFromJsonAsync<CreateSaleResponse>();

            // Act
            var response = await _client.DeleteAsync($"/api/sales/{createResult.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CancelSaleItem_WithExistingSaleAndItem_ShouldReturnOk()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var createRequest = new CreateSaleRequest
            {
                SaleNumber = "SALE-005",
                CustomerId = Guid.NewGuid(),
                CustomerName = "Charlie Brown",
                BranchId = Guid.NewGuid(),
                BranchName = "Branch 5",
                Items = new[]
                {
                    new CreateSaleRequest.SaleItemRequest
                    {
                        ProductId = productId,
                        ProductName = "Product 6",
                        Quantity = 6,
                        UnitPrice = 250m
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);
            var createResult = await createResponse.Content.ReadFromJsonAsync<CreateSaleResponse>();

            // Act
            var response = await _client.DeleteAsync($"/api/sales/{createResult.Id}/items/{productId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
} 