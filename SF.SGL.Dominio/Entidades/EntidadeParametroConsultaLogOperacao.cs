using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.SGL.Dominio.Entidades
{
    public class EntidadeParametroConsultaLogOperacao
    {
        public int? CodigoLogOperacao { get; set; }
     
        public string CodigoIdentificadorUsuario { get; set; }
     
        public DateTime? PeriodoInicial { get; set; }
     
        public DateTime? PeriodoFinal { get; set; }
     
        public TimeSpan? HorarioInicial { get; set; }
     
        public TimeSpan? HorarioFinal { get; set; }
     
        public string Funcionalidade { get; set; }
     
        public int? TipoRegistro { get; set; }
     
        public int? SubTipoRegistro { get; set; }
     
        public string MensagemErro { get; set; }
     
        public string ExcecaoCapturada { get; set; }
    }
}
