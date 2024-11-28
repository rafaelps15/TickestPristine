namespace Tickest.Domain.Contracts.Responses.User
{
    public record CreateUserResponse(Guid Id, string Message = "Usuário criado com sucesso", string ExtraInfo = "")
    {
        // Aqui, os valores padrão podem ser passados diretamente no construtor do record.
        public static CreateUserResponse Create(Guid id, string message = "Usuário criado com sucesso", string extraInfo = "") =>
            new CreateUserResponse(id, message, extraInfo);
    }
}
