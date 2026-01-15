namespace DocumentFlowing.Interfaces.Services;

public interface IAsyncInitialization
{
    /// <summary>
    /// Свойство для асинхронной инициализации
    /// </summary>
    Task Initialization { get; }
}