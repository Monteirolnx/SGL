using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.DeletaSistema
{
    public record DeletaSistemaViewModel : IRequest
    {
        public int Id { get; init; }

        public string Nome { get; init; }

        public string UrlServicoConsultaLog { get; init; }

        public string UsuarioLogin { get; init; }

        public string UsuarioSenha { get; init; }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SF_SGL_Infra.ConfiguracaoEntidades.Sistema.EntidadeSistemaConfig, DeletaSistemaViewModel>();
        }
    }
}
