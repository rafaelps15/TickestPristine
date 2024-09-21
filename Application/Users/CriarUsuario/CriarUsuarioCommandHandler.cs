using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;
using Tickest.Infrastructure.Helpers;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository, 
    IConfiguration configuration) : IRequestHandler<CriarUsuarioCommand>
{
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    private bool SenhaAtendeCritérios(string senha)
    {
        var senhaRegex = new Regex(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$");
        int especialCount = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]").Matches(senha).Count;

        return senhaRegex.IsMatch(senha) && especialCount >= 2;
    }

    public async Task Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        //Verificar se o email é válido
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            throw new TickestException("Email inválido.");

        // Verificar se a senha atende aos critérios
        if (!SenhaAtendeCritérios(request.Senha))
            throw new TickestException("A senha deve ter pelo menos 8 caracteres, incluir pelo menos uma letra maiúscula e dois caracteres especiais.");

        // Verificar se o e-mail já existe
        if (await _usuarioRepository.ExisteUsuarioEmailAsync(request.Email))
            throw new TickestException("Email já cadastrado");

        var senhaSalt = HasherDeSenha.GenerateSalt();
        string senhaCriptografada = HasherDeSenha.HashSenha(request.Senha, senhaSalt);

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = senhaCriptografada,
            Salt = senhaSalt
        };

        await _usuarioRepository.AddAsync(usuario);
    }
}
