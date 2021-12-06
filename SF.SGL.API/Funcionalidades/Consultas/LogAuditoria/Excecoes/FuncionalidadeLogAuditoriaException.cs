namespace SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.Excecoes;

public class FuncionalidadeLogAuditoriaException : Exception
{
    public FuncionalidadeLogAuditoriaException(string erro) : base(erro)
    {
    }

    public static void Quando(bool existeErro, string erro)
    {
        if (existeErro)
        {
            throw new FuncionalidadeLogAuditoriaException(erro);
        }
    }
}
