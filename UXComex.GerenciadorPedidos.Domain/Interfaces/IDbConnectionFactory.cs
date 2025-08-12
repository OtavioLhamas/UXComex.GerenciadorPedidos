using System.Data;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    /// <summary>
    /// Defines an interface for creating and managing database connections.
    /// This abstraction adheres to the Dependency Inversion Principle (DIP).
    /// </summary>
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}