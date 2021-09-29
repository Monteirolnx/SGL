using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.ObtemTodosSistemas
{
    public class ObtemTodosSistemasMediator
    {
        #region Query

        public record ObtemTodosSistemasQuery : IRequest<IEnumerable<ObtemTodosSistemasViewModel>>;

        #endregion
        public class ObtemTodosSistemasQueryHandler : IRequestHandler<ObtemTodosSistemasQuery, IEnumerable<ObtemTodosSistemasViewModel>>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public ObtemTodosSistemasQueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public async Task<IEnumerable<ObtemTodosSistemasViewModel>> Handle(ObtemTodosSistemasQuery request, CancellationToken cancellationToken)
            {
                return await _sglContexto.Sistema
                    .ProjectTo<ObtemTodosSistemasViewModel>(_configurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
