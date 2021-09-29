using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.ObtemSistemaPorId
{
    public record ObtemSistemaPorIdQuery(int Id) : IRequest<ObtemSistemaPorIdModelo>;
}
