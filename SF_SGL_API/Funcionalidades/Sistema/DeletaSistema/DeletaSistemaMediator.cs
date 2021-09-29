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
    public class DeletaSistemaMediator
    {
        #region Query

        public record DeletaSistemaQuery(int Id) : IRequest<DeletaSistemaViewModel>;

        public class DeletaSistemaQueryHandler : IRequestHandler<DeletaSistemaQuery, DeletaSistemaViewModel>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public DeletaSistemaQueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public async Task<DeletaSistemaViewModel> Handle(DeletaSistemaQuery request, CancellationToken cancellationToken)
            {
                return await _sglContexto.Sistema.Where(s => s.Id == request.Id)
                       .ProjectTo<DeletaSistemaViewModel>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);
            }
        }

        #endregion

        #region Command

        public record DeletaSistemaCommand(DeletaSistemaViewModel DeletaSistemaViewModel) : IRequest;

        public class DeletaSistemaCommandHandler : IRequestHandler<DeletaSistemaCommand>
        {
            private readonly SGLContexto _sglContexto;
            public DeletaSistemaCommandHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }

            public async Task<Unit> Handle(DeletaSistemaCommand request, CancellationToken cancellationToken)
            {
                _sglContexto.Sistema.Remove(await _sglContexto.Sistema.FindAsync(new object[] { request.DeletaSistemaViewModel.Id }, cancellationToken: cancellationToken));

                await _sglContexto.SaveChangesAsync(cancellationToken);

                return default;
            }
        }

        #endregion
    }
}
