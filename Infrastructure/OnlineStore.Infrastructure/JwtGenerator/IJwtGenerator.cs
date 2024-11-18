

namespace OnlineStore.Infrastructure.JwtGenerator
{
    /// <summary>
    /// Интерфейс сервиса генератора токенов
    /// </summary>
    public  interface IJwtGenerator
    {
        /// <summary>
        /// Создает и возращает токен доступа
        /// </summary>
        string GenerateToken();
    }
}
