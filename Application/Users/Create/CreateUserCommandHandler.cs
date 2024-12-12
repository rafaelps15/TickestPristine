using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces;

namespace Tickest.Application.Users.Create;

internal sealed class CreateUserCommandHandler(
    IUnitOfWork unitOfWork,
    IPermissionProvider permissionProvider,
    IAuthService authService,
    ILogger<CreateUserCommandHandler> logger)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Início da criação de um novo usuário
        logger.LogInformation("Iniciando a criação de um novo usuário.");

        #region Validação de Permissões
        // Recupera o usuário atual para verificar permissões
        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);

        // Se o usuário não estiver autenticado, lança uma exceção
        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Verifica se o usuário atual tem permissão para criar um novo usuário
        var currentUserId = currentUser.Id;
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUserId, "CreateUser");
        if (!hasPermission)
        {
            logger.LogError("Usuário {UserId} não tem permissão para criar um novo usuário.", currentUserId);
            throw new TickestException("Você não tem permissão para criar um novo usuário.");
        }

        #endregion

        #region Validação de Email
        // Verifica se o email informado já está registrado
        var emailExists = await unitOfWork.Users.DoesEmailExistAsync(command.Email, cancellationToken);
        if (emailExists)
        {
            logger.LogError("Email {Email} já registrado.", command.Email); // Melhora a rastreabilidade
            throw new TickestException("Email já registrado.");
        }
        #endregion

        #region Criação do Novo Usuário
        // Gera um salt e um hash de senha para o novo usuário
        var salt = EncryptionHelper.CreateSaltKey(32);
        var passwordHash = EncryptionHelper.CreatePasswordHashWithSalt(command.Password, salt);

        // Cria o objeto User com os dados do comando
        var user = new User
        {
            Name = command.Name,
            Email = command.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            CreatedAt = DateTime.UtcNow
        };
        #endregion

        #region Adicionando Especialidades e Áreas
        // Se foram fornecidos nomes de especialidades, associa-as ao usuário
        if (command.SpecialtyNames != null && command.SpecialtyNames.Any())
        {
            // Validação de especialidades duplicadas ou inválidas poderia ser inserida aqui
            var specialties = await unitOfWork.SpecialtyRepository
                .GetSpecialtiesByNamesAsync(command.SpecialtyNames, cancellationToken);

            user.UserSpecialties = specialties.Select(s => new UserSpecialty
            {
                UserId = user.Id,
                SpecialtyId = s.Id
            }).ToList();
        }

        // Se foram fornecidos IDs de áreas, associa-as ao usuário
        if (command.AreaIds != null && command.AreaIds.Any())
        {
            // Validação de áreas duplicadas ou inválidas poderia ser inserida aqui
            var areas = await unitOfWork.Repository<Area>()
                .FindAsync(a => command.AreaIds.Contains(a.Id), cancellationToken);

            user.AreaUserSpecialties = areas.Select(a => new AreaUserSpecialty
            {
                UserId = user.Id,
                AreaId = a.Id,
                SpecialtyId = a.SpecialtyId
            }).ToList();
        }
        #endregion

        #region Persistência no Banco de Dados
        // Adiciona o usuário ao repositório e persiste as alterações no banco
        unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // Loga a criação do usuário com sucesso
        logger.LogInformation("Novo usuário criado com ID {UserId}.", user.Id);

        // Melhorar a performance com transações em massa, se possível, caso haja inserções complexas
        #endregion

        // Retorna o ID do novo usuário
        return Result.Success(user.Id);
    }

    #region Melhorias Sugeridas
    // - Adicionar uma validação de formato de email usando uma expressão regular ou um serviço de validação de emails
    // - Incluir um mecanismo para verificar se as especialidades e áreas fornecidas são válidas e não duplicadas
    // - Criar um método para criptografar senhas de forma mais segura com um algoritmo mais robusto (ex: Argon2 ou PBKDF2)
    // - Criar logs de auditoria para rastrear alterações e atividades dos usuários (quem criou o usuário, quando, etc.)
    // - Incluir um mecanismo de notificação para o usuário após a criação (email, mensagem na tela, etc.)
    // - Usar um mecanismo de verificação de conflitos entre áreas e especialidades de forma mais flexível
    // - Testar e garantir que a lógica de permissões seja adequada para diferentes cenários de usuários
    #endregion
}
