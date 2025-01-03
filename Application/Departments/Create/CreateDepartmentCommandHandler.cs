﻿using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Departments.Create;

internal sealed class CreateDepartmentCommandHandler(
    IPermissionProvider permissionProvider,
    IAuthService authService,
    IDepartmentRepository departmentRepository,
    IUserRepository userRepository,
    ILogger<CreateDepartmentCommandHandler> logger)
    : ICommandHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo departamento.");

        #region Verificação de Permissão do Usuário

        // Obtém o usuário autenticado com todas as informações necessárias via token
        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        var currentUserId = currentUser.Id;

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Verificando se o usuário tem permissão 
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUser, "CreateDepartment");
        if (!hasPermission)
        {
            logger.LogWarning("Usuário {UserId} não tem permissão para criar um departamento.", currentUserId);
            throw new TickestException("Usuário não tem permissão para criar um departamento.");
        }

        logger.LogInformation("Usuário {UserId} tem permissão para criar um departamento.", currentUserId);

        #endregion

        #region Validação do Usuário Responsável (Gestor do Departamento)

        if (command.DepartmentManagerId.HasValue)
        {
            // Validação para garantir que o usuário fornecido exista
            var departmentManager = await userRepository.GetByIdAsync(command.DepartmentManagerId.Value, cancellationToken);
            if (departmentManager == null)
            {
                logger.LogError("Gestor do departamento com ID {UserId} não encontrado.", command.DepartmentManagerId);
                throw new TickestException("Gestor do departamento não encontrado.");
            }
        }

        #endregion

        #region Criação do Novo Departamento

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            SectorId = command.SectorId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        if (command.DepartmentManagerId.HasValue)
        {
            department.DepartmentManagerId = command.DepartmentManagerId.Value;
        }

        logger.LogInformation("Departamento preparado para persistência: {DepartmentId}.", department.Id);

        #endregion

        #region Persistência no Banco de Dados

        // Salvando o departamento no repositório
        await departmentRepository.AddAsync(department, cancellationToken);
        await departmentRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"Departamento {department.Name} criado com sucesso.");

        #endregion

        return Result.Success(department.Id); // Retorna o ID do departamento criado
    }
}
