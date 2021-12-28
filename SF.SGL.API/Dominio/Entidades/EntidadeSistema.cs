namespace SF.SGL.API.Dominio.Entidades;

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
        DomainExceptionValidation.Quando(nome.Length < 3, "Nome de sistema deve ter ao menos 3 caracteres.");
        DomainExceptionValidation.Quando(nome.Length > 100, "Nome de sistema não deve ter mais do que 100 caracteres.");

        return nome;
    }

    private static string ValidarUrlServicoConsultaLog(string urlServicoConsultaLog)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(urlServicoConsultaLog), "Url de serviço de consulta de log do sistema inválida.");

        bool uriValida = Uri.IsWellFormedUriString(urlServicoConsultaLog, UriKind.Absolute);
        DomainExceptionValidation.Quando(!uriValida, "Url de serviço de consulta de log do sistema inválida.");

        return urlServicoConsultaLog;
    }

    private static string ValidarUsuarioLogin(string usuarioLogin)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(usuarioLogin), "Login do usuário do sistema inválido.");
        DomainExceptionValidation.Quando(usuarioLogin.Length > 10, "Login do usuário do sistema não deve ter mais do que 10 caracteres.");

        return usuarioLogin;
    }

    private static string ValirdarUsuarioSenha(string pUsuarioSenha)
    {
        DomainExceptionValidation.Quando(string.IsNullOrEmpty(pUsuarioSenha), "Senha do usuário do sistema inválida.");
        DomainExceptionValidation.Quando(pUsuarioSenha.Length > 10, "Senha do usuário do sistema não deve ter mais do que 10 caracteres.");

        return pUsuarioSenha;
    }
}