using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;
using UXComex.GerenciadorPedidos.Domain.Services;
using Xunit;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _service = new ProductService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenNameMissing()
    {
        var product = new Product
        {
            Name = "",
            Price = 10,
            StockQuantity = 5
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(product));
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenPriceInvalid()
    {
        var product = new Product
        {
            Name = "Test",
            Price = 0,
            StockQuantity = 5
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(product));
    }

    [Fact]
    public async Task CreateAsync_ReturnsId_WhenValid()
    {
        var product = new Product
        {
            Name = "Test",
            Price = 10,
            StockQuantity = 5
        };
        _repoMock.Setup(r => r.AddAsync(product)).ReturnsAsync(1);

        var id = await _service.CreateAsync(product);

        Assert.Equal(1, id);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenProductNotFound()
    {
        var product = new Product
        {
            Id = 99,
            Name = "Test",
            Price = 10,
            StockQuantity = 5
        };
        _repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync((Product)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(product));
    }

    [Fact]
    public async Task UpdateAsync_CallsRepository_WhenProductExists()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Test",
            Price = 10,
            StockQuantity = 5
        };
        _repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        await _service.UpdateAsync(product);

        _repoMock.Verify(r => r.UpdateAsync(product), Times.Once);
    }
}