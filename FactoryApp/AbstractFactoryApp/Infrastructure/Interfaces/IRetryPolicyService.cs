namespace AbstractFactoryApp.Infrastructure.Interfaces;

public interface IRetryPolicyService
{
    Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3);
    Task ExecuteWithRetryAsync(Func<Task> operation, int maxRetries = 3);
}
