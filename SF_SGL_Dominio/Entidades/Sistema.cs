using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_SGL_Dominio.Entidades
{
    public sealed class Sistema
    {
        public int Id { get; private set; }

        public string Nome { get; private set; }

        public string UrlServicoConsultaLog { get; private set; }

        public string UsuarioLogin { get; private set; }

        public string UsuarioSenha { get; private set; }
    }
}
