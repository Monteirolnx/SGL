using System;

namespace SF.SGL.Dominio.Validacao
{
    public class DomainExceptionValidation : Exception
    {
        public DomainExceptionValidation(string pErro) : base(pErro)
        {
        }

        public static void Quando(bool pExisteErro, string pErro)
        {
            if (pExisteErro)
            {
                throw new DomainExceptionValidation(pErro);
            }
        }
    }
}
