namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.Excecoes;

public class FuncionalidadeMonitoramentoException : Exception
{
    public FuncionalidadeMonitoramentoException(string erro) : base(erro)
    {
    }

    public static void Quando(bool existeErro, string erro)
    {
        if (existeErro)
        {
            throw new FuncionalidadeMonitoramentoException(erro);
        }
    }
}
