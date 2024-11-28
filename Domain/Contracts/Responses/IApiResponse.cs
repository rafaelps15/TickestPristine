namespace Tickest.Domain.Contracts.Responses
{
    /// <summary>
    /// Interface para respostas genéricas da API, garantindo que todas as respostas possuam propriedades comuns.
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>
        /// Identificador único da resposta (pode ser utilizado para rastrear ou identificar a resposta).
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Mensagem associada à resposta, geralmente usada para fornecer informações adicionais (como status ou erro).
        /// </summary>
        string Message { get;} 
    }
}
