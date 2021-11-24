namespace SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao;

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
