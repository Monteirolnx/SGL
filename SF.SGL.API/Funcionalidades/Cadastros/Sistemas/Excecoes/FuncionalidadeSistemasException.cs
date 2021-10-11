using System;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.Excecoes
{
    public class FuncionalidadeSistemasException : Exception
    {
        public FuncionalidadeSistemasException(string erro) : base(erro)
        {
        }

        public static void Quando(bool existeErro, string erro)
        {
            if (existeErro)
            {
                throw new FuncionalidadeSistemasException(erro);
            }
        }
    }
}
