using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL.Data;

namespace SF_SGL.Features.Sistema
{
    public class AdicionaSistema
    {
        public class Command : IRequest<int>
        {
            [Required]
            public string Nome { get; set; }

            [Required]
            public string UrlServicoConsultaLog { get; set; }

            [Required]
            public string UsuarioLogin { get; set; }

            [Required]
            public string UsuarioSenha { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly SGLContext _sglContext;

            public Handler(SGLContext sglContext)
            {
                _sglContext = sglContext;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                Entidades.Sistema sistema = new()
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
}
