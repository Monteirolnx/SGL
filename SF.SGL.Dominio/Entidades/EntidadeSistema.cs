using SF.SGL.Dominio.Validacao;

namespace SF.SGL.Dominio.Entidades
{
    public class EntidadeSistema
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string UrlServicoConsultaLog { get; set; }

        public string UsuarioLogin { get; set; }

        public string UsuarioSenha { get; set; }

        public EntidadeSistema(string nome, string urlServicoConsultaLog, string usuarioLogin, string usuarioSenha)
        {
            ValidarDominio(nome, urlServicoConsultaLog, usuarioLogin, usuarioSenha);
        }

        private void ValidarDominio(string nome, string urlServicoConsultaLog, string usuarioLogin, string usuarioSenha)
        {
            ValidarNome(nome);
            ValidarUrlServicoConsultaLog(urlServicoConsultaLog);
            ValidarUsuarioLogin(usuarioLogin);
            ValirdarUsuarioSenha(usuarioSenha);
        }
        private void ValidarNome(string nome)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(nome), "Nome de sistema inválido.");
            DomainExceptionValidation.Quando(nome.Length < 3, "Nome de sistema deve conter ao menos 3 caracteres.");
            Nome = nome;
        }

        private void ValidarUrlServicoConsultaLog(string urlServicoConsultaLog)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(urlServicoConsultaLog), "Url de serviço de consulta de log do sistema inválida.");
            UrlServicoConsultaLog = urlServicoConsultaLog;
        }

        private void ValidarUsuarioLogin(string usuarioLogin)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(usuarioLogin), "Login do usuário do sistema inválido.");
            UsuarioLogin = usuarioLogin;
        }

        private void ValirdarUsuarioSenha(string pUsuarioSenha)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(pUsuarioSenha), "Senha do usuário do sistama inválida.");
            UsuarioSenha = pUsuarioSenha;
        }

    }
}
