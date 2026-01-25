namespace DocumentFlowing.Interfaces.Client;

public interface IGeneralClient
{
    /// <summary>
    /// Базовый метод для эндпоинтов Patch
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="uri">эндпоинт</param>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse?> PatchResponseAsync<TRequest, TResponse>(TRequest request, string uri);
    
    /// <summary>
    /// Базовый метод для эндпоинтов Post
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="uri">эндпоинт</param>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse?> PostResponseAsync<TRequest, TResponse>(TRequest request, string uri);
    
    /// <summary>
    /// Базовый метод для эндпоинтов Delete
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="uri">эндпоинт</param>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse?> DeleteResponseAsync<TRequest, TResponse>(TRequest request, string uri);
    
    /// <summary>
    /// Базовый метод для эндпоинтов Get
    /// </summary>
    /// <param name="uri">эндпоинт</param>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <returns>TResponse</returns>
    Task<TResponse?> GetResponseAsync<TResponse>(string uri);
}