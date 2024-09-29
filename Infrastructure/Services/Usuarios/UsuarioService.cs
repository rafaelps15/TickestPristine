using Microsoft.AspNetCore.Identity;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Helpers;
using Tickest.Infrastructure.Interfaces;
using Tickest.Persistence.Repositories;

namespace Tickest.Infrastructure.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
        => _usuarioRepository = usuarioRepository;

    public async Task<bool> ExisteEmailCadastroAsync(string email)
        => await _usuarioRepository.ExisteEmailCadastroAsync(email);

    public async Task<Usuario> ObterUsuarioPorEmailAsync(string email)
        => await _usuarioRepository.ObterUsuarioPorEmailAsync(email)
            ?? throw new TickestException("Usuário não encontrado.");

    public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
    {
        var usuario = await ObterUsuarioPorEmailAsync(email);

        if (usuario == null)
            throw new TickestException("Usuário não encontrado.");
    
        var hasher = new HasherDeSenha();
        var hashedPassword = hasher.HashSenha(senha, usuario.Salt);

        if (!hashedPassword.Equals(usuario.Senha))
            throw new TickestException("Senha incorreta.");

        return usuario;
    }
}

