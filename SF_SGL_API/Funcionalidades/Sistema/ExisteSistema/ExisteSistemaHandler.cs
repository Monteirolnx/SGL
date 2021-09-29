using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.ExisteSistema
{
    public class ExisteSistemaHandler : IRequestHandler<ExisteSistemaQuery, bool>
    {
        private readonly SGLContext _sglContext;
        private readonly IConfigurationProvider _configurationProvider;

        public ExisteSistemaHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
        {
            _sglContext = sglContext;
            _configurationProvider = configurationProvider;
        }
        public Task<bool> Handle(ExisteSistemaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_sglContext.Sistema.Where(s => s.Id == request.Id).Any());
        }
    }
}
