using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SF_SGL_API.Funcionalidades.Sistemas
{
    public class SistemasException: Exception
    {
        public SistemasException(string erro): base(erro)
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
