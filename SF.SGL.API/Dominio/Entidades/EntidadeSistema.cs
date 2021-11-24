namespace SF.SGL.API.Dominio.Entidades;

// TODO: Realizar validação entidade vs tela.
public class EntidadeSistema
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string UrlServicoConsultaLog { get; set; }

    public string UsuarioLogin { get; set; }

    public string UsuarioSenha { get; set; }

    public EntidadeSistema(string nome, string urlServicoConsultaLog, string usuarioLogin, string usuarioSenha)
    {
        Nome = ValidarNome(nome);
        UrlServicoConsultaLog = ValidarUrlServicoConsultaLog(urlServicoConsultaLog);
        UsuarioLogin = ValidarUsuarioLogin(usuarioLogin);
        UsuarioSenha = ValirdarUsuarioSenha(usuarioSenha);
    }

    private static string ValidarNome(string nome)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(nome), "Nome de sistema inválido.");
        DomainExceptionValidation.Quando(nome.Length < 3, "Nome de sistema deve conter ao menos 3 caracteres.");
        return nome;
    }

    private static string ValidarUrlServicoConsultaLog(string urlServicoConsultaLog)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(urlServicoConsultaLog), "Url de serviço de consulta de log do sistema inválida.");
        return urlServicoConsultaLog;
    }

    private static string ValidarUsuarioLogin(string usuarioLogin)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(usuarioLogin), "Login do usuário do sistema inválido.");
        return usuarioLogin;
    }

    private static string ValirdarUsuarioSenha(string pUsuarioSenha)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(pUsuarioSenha), "Senha do usuário do sistama inválida.");
        return pUsuarioSenha;
    }

}
