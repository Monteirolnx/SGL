using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.EditaSistema
{
    public class EditaSistemaHandler : IRequestHandler<EditaSistemaQuery, EditaSistemaModelo>
    {
        private readonly SGLContext _sglContext;
        private readonly IConfigurationProvider _configurationProvider;

        public EditaSistemaHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
        {
            _sglContext = sglContext;
            _configurationProvider = configurationProvider;
        }

        public Task<EditaSistemaModelo> Handle(EditaSistemaQuery request, CancellationToken cancellationToken)
        {
            return _sglContext.Sistema.Where(s => s.Id == request.Id)
                .ProjectTo<EditaSistemaModelo>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}

