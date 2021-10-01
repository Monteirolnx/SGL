namespace SF.SGL.Dominio.Entidades
{
    public class EntidadeBase
    {
        public int Id { get; protected set; }

        public void ValidarDominioBase(int id)
        {
            ValidarId(id);
        }

        private void ValidarId(int id)
        {
            DomainExceptionValidation.Quando(id < 0, "Id inválido.");
            Id = id;
        }
    }
}
