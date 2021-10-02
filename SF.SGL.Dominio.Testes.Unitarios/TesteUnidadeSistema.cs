using System;
using FluentAssertions;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Dominio.Validacao;
using Xunit;

namespace SF.SGL.Dominio.Testes.Unitarios
{
    public class TesteUnidadeSistema
    {
        [Fact(DisplayName = "Criar objeto sistema com estado válido")]
        public void CriarObjEntidadeSistema_ComParametrosValidos_EstadoValido()
        {
            Action action = () =>
            {
                EntidadeSistema entidadeSistema = new("Nome sistema", @"http://localhost:53463", "Usuariologin", "Usuariosenha");
            };
            action.Should()
                .NotThrow<DomainExceptionValidation>();
        }


        [Fact(DisplayName = "Criar sistema com nome inválido")]
        public void CriarObjEntidadeSistema_ComParametrosInvalidosValidos_DomainExceptionNomeInvalido()
        {
            Action action = () =>
            {
                EntidadeSistema entidadeSistema = new(null, @"http://localhost:53463", "Usuariologin", "Usuariosenha");
            };
            action.Should()
                .Throw<DomainExceptionValidation>();

            action = () =>
           {
               EntidadeSistema entidadeSistema = new("a", @"http://localhost:53463", "Usuariologin", "Usuariosenha");
           };
            action.Should()
                .Throw<DomainExceptionValidation>();

        }

        [Fact(DisplayName = "Criar sistema com url de serviço de consulta de log inválida.")]
        public void CriarObjEntidadeSistema_ComParametrosInvalidosValidos_DomainExceptionUrlInvalida()
        {
            Action action = () =>
            {
                EntidadeSistema entidadeSistema = new("Nome sistema", null, "Usuariologin", "Usuariosenha");
            };
            action.Should()
                .Throw<DomainExceptionValidation>()
                .WithMessage("Url de serviço de consulta de log do sistema inválida.");
        }

    }

}
