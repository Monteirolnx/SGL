using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using SF_SGL_Infra.ConfiguracaoEntidades.Sistema;

namespace SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema
{
    public class AdicionaSistemaInputModel : IRequest<int>
    {
        [Required(ErrorMessage ="Nome do sistema é obrigatório.")]
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
            CreateMap<EntidadeSistemaConfig, AdicionaSistemaInputModel>();
        }
    }
}
