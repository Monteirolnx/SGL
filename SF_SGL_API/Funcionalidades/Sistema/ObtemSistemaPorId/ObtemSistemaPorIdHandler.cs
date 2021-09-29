using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.ObtemSistemaPorId
{
    public class ObtemSistemaPorIdHandler: IRequestHandler<ObtemSistemaPorIdQuery, ObtemSistemaPorIdModelo>
    {
        private readonly SGLContext _sglContext;
        private readonly IConfigurationProvider _configurationProvider;

        public ObtemSistemaPorIdHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
        {
            _sglContext = sglContext;
            _configurationProvider = configurationProvider;
        }
        public Task<ObtemSistemaPorIdModelo> Handle(ObtemSistemaPorIdQuery request, CancellationToken cancellationToken)
        {
            return _sglContext.Sistema.Where(s => s.Id == request.Id)
                .ProjectTo<ObtemSistemaPorIdModelo>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
