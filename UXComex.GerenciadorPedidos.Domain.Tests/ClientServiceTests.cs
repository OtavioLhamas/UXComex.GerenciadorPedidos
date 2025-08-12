using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;
using UXComex.GerenciadorPedidos.Domain.Services;
using Xunit;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _repoMock;
    private readonly ClientService _service;

    public ClientServiceTests()
    {
        _repoMock = new Mock<IClientRepository>();
        _service = new ClientService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenEmailExists()
    {
        var client = new Client
        {
            Name = "Test",
            Email = "test@example.com",
            Phone = "123456789",
            RegistrationDate = DateTime.Now
        };
        _repoMock.Setup(r => r.GetByEmailAsync(client.Email)).ReturnsAsync(client);

        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(client));
    }

    [Fact]
    public async Task CreateAsync_ReturnsId_WhenValid()
    {
        var client = new Client
        {
            Name = "Test",
            Email = "unique@example.com",
            Phone = "123456789",
            RegistrationDate = DateTime.Now
        };
        _repoMock.Setup(r => r.GetByEmailAsync(client.Email)).ReturnsAsync((Client)null);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Client>())).ReturnsAsync(42);

        var id = await _service.CreateAsync(client);

        Assert.Equal(42, id);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenClientNotFound()
    {
        var client = new Client
        {
            Id = 99,
            Name = "Test",
            Email = "test@example.com",
            Phone = "123456789",
            RegistrationDate = DateTime.Now
        };
        _repoMock.Setup(r => r.GetByIdAsync(client.Id)).ReturnsAsync((Client)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(client));
    }

    [Fact]
    public async Task UpdateAsync_CallsRepository_WhenClientExists()
    {
        var client = new Client
        {
            Id = 1,
            Name = "Test",
            Email = "test@example.com",
            Phone = "123456789",
            RegistrationDate = DateTime.Now
        };
        _repoMock.Setup(r => r.GetByIdAsync(client.Id)).ReturnsAsync(client);

        await _service.UpdateAsync(client);

        _repoMock.Verify(r => r.UpdateAsync(client), Times.Once);
    }
}