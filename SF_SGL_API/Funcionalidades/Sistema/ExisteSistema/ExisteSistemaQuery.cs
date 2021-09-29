using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.ExisteSistema
{
    public record ExisteSistemaQuery(int Id) : IRequest<bool>;
}
