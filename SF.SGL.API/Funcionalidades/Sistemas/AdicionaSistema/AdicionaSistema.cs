using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Infra.Data.Contextos;

namespace SF.SGL.API.Funcionalidades.Sistemas
{
    public class AdicionaSistema
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
                CreateMap<EntidadeSistema, Command>().ReverseMap();
            }
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IMapper _mapper;

            public CommandHandler(SGLContexto sglContexto, IMapper mapper)
            {
                _sglContexto = sglContexto;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                EntidadeSistema entidadeSistema = _mapper.Map<EntidadeSistema>(command); 

                await _sglContexto.AddAsync(entidadeSistema, cancellationToken);

                await _sglContexto.SaveChangesAsync(cancellationToken);

                return entidadeSistema.Id;
            }
        }
    }
}
