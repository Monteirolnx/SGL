using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Infra.Data.Contexto;

namespace SF.SGL.API.Funcionalidades.Sistemas.AdicionaSistema
{
    public class AdicionaSistemaMediator
    {

        public class Command : IRequest<int>
        {
            [Required(ErrorMessage = "Nome do sistema é obrigatório.")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "Url do serviço do sistema é obrigatório.")]
            public string UrlServicoConsultaLog { get; set; }

            [Required(ErrorMessage = "Login do usuário do sistema é obrigatório.")]
            public string UsuarioLogin { get; set; }

            [Required(ErrorMessage = "Senha do usuário do sistema é obrigatório.")]
            public string UsuarioSenha { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<SistemaEntidade, Command>();
            }
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly SGLContexto _sglContexto;

            public CommandHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                SistemaEntidade sistemaEntidade = new()
                {
                    Nome = command.Nome,
                    UrlServicoConsultaLog = command.UrlServicoConsultaLog,
                    UsuarioLogin = command.UsuarioLogin,
                    UsuarioSenha = command.UsuarioSenha
                };
                await _sglContexto.AddAsync(sistemaEntidade, cancellationToken);

                await _sglContexto.SaveChangesAsync(cancellationToken);

                return sistemaEntidade.Id;
            }
        }
    }
}
