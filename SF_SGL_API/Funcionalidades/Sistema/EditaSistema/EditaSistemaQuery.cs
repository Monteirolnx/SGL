using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.EditaSistema
{
    public record EditaSistemaQuery(int Id) : IRequest<EditaSistemaModelo>;
  
}
