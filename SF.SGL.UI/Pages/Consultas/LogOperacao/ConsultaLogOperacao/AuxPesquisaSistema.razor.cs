using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using static SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao.ConsultaLogOperacao;

namespace SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao
{
    public partial class AuxPesquisaSistema
    {
        [Parameter] public List<Sistema> Sistemas { get; set; }
        protected bool DesabilitarOk { get; set; } = true;

        protected IList<Sistema> SistemaSelecionado { get; set; }

        #region Injects
        [Inject]
        protected RadzenDataGrid<Sistema> GridPesquisaSistemas { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }
        #endregion
    }
}
