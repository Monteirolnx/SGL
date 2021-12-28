namespace SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.Excecoes;

public class FuncionalidadeLogMonitoramentoException : Exception
{
    public FuncionalidadeLogMonitoramentoException(string erro) : base(erro)
    {
    }

    public static void Quando(bool existeErro, string erro)
    {
        if (existeErro)
        {
            throw new FuncionalidadeLogMonitoramentoException(erro);
        }
    }
}
