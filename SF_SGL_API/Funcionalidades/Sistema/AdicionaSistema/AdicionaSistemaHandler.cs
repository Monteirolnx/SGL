using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema
{
    public class AdicionaSistemaHandler : IRequestHandler<AdicionaSistemaCommand, int>
    {
        private readonly SGLContext _sglContext;

        public AdicionaSistemaHandler(SGLContext sglContext)
        {
            _sglContext = sglContext;
        }

        public async Task<int> Handle(AdicionaSistemaCommand request, CancellationToken cancellationToken)
        {
            SF_SGL_Infra.ConfiguracaoEntidades.Sistema.Sistema sistema = new()
            {
                Nome = request.AdicionamodeloSistemaModelo.Nome,
                UrlServicoConsultaLog = request.AdicionamodeloSistemaModelo.UrlServicoConsultaLog,
                UsuarioLogin = request.AdicionamodeloSistemaModelo.UsuarioLogin,
                UsuarioSenha = request.AdicionamodeloSistemaModelo.UsuarioSenha
            };
            await _sglContext.AddAsync(sistema, cancellationToken);

            await _sglContext.SaveChangesAsync(cancellationToken);

            return sistema.Id;
        }
    }
}
