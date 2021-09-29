using System.Collections.Generic;
using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.ObtemTodosSistemas
{
    public record ObtemTodosSistemasQuery : IRequest<List<ObtemTodosSistemasModelo>>;
  
}
