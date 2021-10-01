using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.SGL.Dominio.Entidades.Sistema
{
    public class EntidadeSistema : EntidadeBase
    {
        public string Nome { get; set; }

        public string UrlServicoConsultaLog { get; set; }

        public string UsuarioLogin { get; set; }

        public string UsuarioSenha { get; set; }

        public EntidadeSistema(int id, string nome, string urlServicoConsultaLog, string usuarioLogin, string usuarioSenha)
        {
            ValidarDominio(id, nome, urlServicoConsultaLog, usuarioLogin, usuarioSenha);  
        }

        private void ValidarDominio(int id, string nome, string urlServicoConsultaLog, string usuarioLogin, string usuarioSenha)
        {
            ValidarDominioBase(id);
            ValidarNome(nome);
            ValidarUrlServicoConsultaLog(urlServicoConsultaLog);
            ValidarUsuarioLogin(usuarioLogin);
            ValirdarUsuarioSenha(usuarioSenha);
        }
        private void ValidarNome(string pNome)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(pNome), "Nome de sistema inválido.");
            Nome = pNome;
        }

        private void ValidarUrlServicoConsultaLog(string pUrlServicoConsultaLog)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(pUrlServicoConsultaLog), "Url de serviço de consulta de log do sistema inválida.");
            UrlServicoConsultaLog = pUrlServicoConsultaLog;
        }

        private void ValidarUsuarioLogin(string pUsuarioLogin)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(pUsuarioLogin), "Login do usuário do sistema inválido.");
            UsuarioLogin = pUsuarioLogin;
        }

        private void ValirdarUsuarioSenha(string pUsuarioSenha)
        {
            DomainExceptionValidation.Quando(string.IsNullOrEmpty(pUsuarioSenha), "Senha do usuário do sistama inválida.");
            UsuarioSenha = pUsuarioSenha;
        }

        public void ValidarAtualizacao(int pId, string pNome, string pUrlServicoConsultaLog, string pUsuarioLogin, string pUsuarioSenha)
        {
            ValidarDominio(pId, pNome, pUrlServicoConsultaLog, pUsuarioLogin, pUsuarioSenha);
        }
    }
}
