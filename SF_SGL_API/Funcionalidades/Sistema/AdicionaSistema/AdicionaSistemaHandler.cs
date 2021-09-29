using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema
{
    public class AdicionaSistemaHandler : IRequestHandler<AdicionaSistemaModelo, int>
    {
        private readonly SGLContext _sglContext;

        public AdicionaSistemaHandler(SGLContext sglContext)
        {
            _sglContext = sglContext;
        }

        public async Task<int> Handle(AdicionaSistemaModelo request, CancellationToken cancellationToken)
        {
            SF_SGL_Infra.ConfiguracaoEntidades.Sistema.Sistema sistema = new()
            {
                Nome = request.Nome,
                UrlServicoConsultaLog = request.UrlServicoConsultaLog,
                UsuarioLogin = request.UsuarioLogin,
                UsuarioSenha = request.UsuarioSenha
            };
            await _sglContext.AddAsync(sistema, cancellationToken);

            await _sglContext.SaveChangesAsync(cancellationToken);

            return sistema.Id;
        }
    }
}
