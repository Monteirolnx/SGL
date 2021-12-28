namespace SF.SGL.API.Funcionalidades.Cadastros.ExecucaoMonitoramento.Excecoes;
public class FuncionalidadeExecucaoMonitoramentoException : Exception
{
    public FuncionalidadeExecucaoMonitoramentoException(string erro) : base(erro)
    {
    }

    public static void Quando(bool existeErro, string erro)
    {
        if (existeErro)
        {
            throw new FuncionalidadeExecucaoMonitoramentoException(erro);
        }
    }
}
