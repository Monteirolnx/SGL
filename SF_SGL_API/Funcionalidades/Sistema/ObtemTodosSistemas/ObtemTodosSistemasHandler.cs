using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.ObtemTodosSistemas
{
    public class ObtemTodosSistemasHandler : IRequestHandler<ObtemTodosSistemasQuery, List<ObtemTodosSistemasModelo>>
    {
        private readonly SGLContext _sglContext;
        private readonly IConfigurationProvider _configurationProvider;

        public ObtemTodosSistemasHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
        {
            _sglContext = sglContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<List<ObtemTodosSistemasModelo>> Handle(ObtemTodosSistemasQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_sglContext.Sistema.ProjectTo<ObtemTodosSistemasModelo>(_configurationProvider).ToList());
        }
    }
}
