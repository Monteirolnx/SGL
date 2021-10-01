using System;

namespace SF.SGL.API.Funcionalidades.Sistemas
{
    public class SistemasException : Exception
    {
        public SistemasException(string erro) : base(erro)
        {
        }

        public static void Quando(bool existeErro, string erro)
        {
            if (existeErro)
            {
                throw new SistemasException(erro);
            }
        }
    }
}
