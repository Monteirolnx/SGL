using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.DeletaSistema
{

    public class DeletaSistemaHandler : IRequestHandler<DeletaSistemaQuery, DeletaSistemaModelo>
    {
        private readonly SGLContext _sglContext;
        private readonly IConfigurationProvider _configurationProvider;

        public DeletaSistemaHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
        {
            _sglContext = sglContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<DeletaSistemaModelo> Handle(DeletaSistemaQuery request, CancellationToken cancellationToken)
        {
            return await _sglContext.Sistema.Where(s => s.Id == request.Id)
                   .ProjectTo<DeletaSistemaModelo>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);
        }
    }
}
