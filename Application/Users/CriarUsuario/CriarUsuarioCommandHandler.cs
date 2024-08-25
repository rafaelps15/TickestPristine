using MediatR;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository=usuarioRepository;
    }

    public async Task Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        //Verificar se o email é válido

        //verificar se a senha está de acordo com as regras do sistema

        //verificar se email já existe
        if(await _usuarioRepository.ExisteUsuarioEmailAsync(request.Email))
        {
            throw new TickestException("Email já cadastrado");
        }

        //criptografar senha
        //..


        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = string.Empty, //senha criptografada
            Salt = string.Empty, //salt criado para senha
        };

        await _usuarioRepository.AddAsync(usuario);
    }
}
