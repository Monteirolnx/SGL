using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.DeletaSistema
{
    public class DeleteSistemaCommandHandler : IRequestHandler<DeletaSistemaModelo>
    {
        private readonly SGLContext _sglContext;
        public DeleteSistemaCommandHandler(SGLContext sglContext)
        {
            _sglContext = sglContext;
        }

        public async Task<Unit> Handle(DeletaSistemaModelo request, CancellationToken cancellationToken)
        {
            _sglContext.Sistema.Remove(await _sglContext.Sistema.FindAsync(request.Id));

            await _sglContext.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}
