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
    public class ObtemSistemaPorIdMediator
    {
        #region Query

        public record ObtemSistemaPorIdQuery(int Id) : IRequest<ObtemSistemaPorIdViewModel>;
        
        public class ObtemSistemaPorIdHandler : IRequestHandler<ObtemSistemaPorIdQuery, ObtemSistemaPorIdViewModel>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public ObtemSistemaPorIdHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }
            public Task<ObtemSistemaPorIdViewModel> Handle(ObtemSistemaPorIdQuery request, CancellationToken cancellationToken)
            {
                return _sglContexto.Sistema.Where(s => s.Id == request.Id)
                    .ProjectTo<ObtemSistemaPorIdViewModel>(_configurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

        #endregion
    }
}
