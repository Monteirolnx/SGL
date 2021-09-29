using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_SGL_Infra.ConfiguracaoEntidades.Sistema
{
    public class Sistema
    {
        [Column("cd_sistema")]
        public int Id { get; set; }

        [Column("nm_sistema")]
        public string Nome { get; set; }
                
        [Column("nm_url_servico_consulta_log")]
        public string UrlServicoConsultaLog { get; set; }
                
        [Column("nm_usuario_servico_consulta_log")]
        public string UsuarioLogin { get; set; }
                
        [Column("nm_senha_servico_consulta_log")]
        public string UsuarioSenha { get; set; }
    }
}
