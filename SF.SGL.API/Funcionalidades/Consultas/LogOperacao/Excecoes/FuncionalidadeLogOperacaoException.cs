using System;

namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.Excecoes
{
    public class FuncionalidadeLogOperacaoException : Exception
    {
        public FuncionalidadeLogOperacaoException(string erro) : base(erro)
        {
        }

        public static void Quando(bool existeErro, string erro)
        {
            if (existeErro)
            {
                throw new FuncionalidadeLogOperacaoException(erro);
            }
        }
    }
}
