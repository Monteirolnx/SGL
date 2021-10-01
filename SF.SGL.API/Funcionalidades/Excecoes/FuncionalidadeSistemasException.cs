using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SF.SGL.API.Funcionalidades.Excecoes
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
