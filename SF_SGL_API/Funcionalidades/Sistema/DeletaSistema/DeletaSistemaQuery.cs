using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.DeletaSistema
{
    public record DeletaSistemaQuery(int Id) : IRequest<DeletaSistemaModelo>;
    
}
