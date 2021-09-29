using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.EditaSistema
{
    public class EditaSistemaCommandHandler : IRequestHandler<EditaSistemaModelo>
    {
        private readonly SGLContext _sglContext;
     
        public EditaSistemaCommandHandler(SGLContext sglContext)
        {
            _sglContext = sglContext;
        }
        public async Task<Unit> Handle(EditaSistemaModelo request, CancellationToken cancellationToken)
        {
            SF_SGL_Infra.ConfiguracaoEntidades.Sistema.Sistema sistema = await _sglContext.Sistema.FindAsync(request.Id);

            sistema.Nome = request.Nome;
            sistema.UrlServicoConsultaLog = request.UsuarioLogin;
            sistema.UsuarioLogin = request.UsuarioLogin;
            sistema.UsuarioSenha = request.UsuarioSenha;

            _sglContext.Sistema.Update(sistema);
            await _sglContext.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}
