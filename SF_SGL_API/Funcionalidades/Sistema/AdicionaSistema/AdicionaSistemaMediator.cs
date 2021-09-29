using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL_Infra.ConfiguracaoEntidades.Sistema;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema
{
    public class AdicionaSistemaMediator
    {
        #region Command

        public record AdicionaSistemaCommand(AdicionaSistemaInputModel AdicionaSistemaInputModel) : IRequest<int>;

        public class AdicionaSistemaCommandHandler : IRequestHandler<AdicionaSistemaCommand, int>
        {
            private readonly SGLContexto _sglContexto;

            public AdicionaSistemaCommandHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }

            public async Task<int> Handle(AdicionaSistemaCommand request, CancellationToken cancellationToken)
            {
                EntidadeSistemaConfig sistema = new()
                {
                    Nome = request.AdicionaSistemaInputModel.Nome,
                    UrlServicoConsultaLog = request.AdicionaSistemaInputModel.UrlServicoConsultaLog,
                    UsuarioLogin = request.AdicionaSistemaInputModel.UsuarioLogin,
                    UsuarioSenha = request.AdicionaSistemaInputModel.UsuarioSenha
                };
                await _sglContexto.AddAsync(sistema, cancellationToken);

                await _sglContexto.SaveChangesAsync(cancellationToken);

                return sistema.Id;
            }
        }

        #endregion
    }
}
