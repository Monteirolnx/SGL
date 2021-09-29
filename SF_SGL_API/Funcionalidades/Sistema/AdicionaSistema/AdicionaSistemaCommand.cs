using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema
{
    public record AdicionaSistemaCommand(AdicionaSistemaModelo adicionamodeloSistemaModelo) : IRequest<int>;
  
}
