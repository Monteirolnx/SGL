﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema
{
    public class AdicionaSistemaModelo : IRequest<int>
    {
        public string Nome { get; set; }

        public string UrlServicoConsultaLog { get; set; }

        public string UsuarioLogin { get; set; }

        public string UsuarioSenha { get; set; }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SF_SGL_Infra.ConfiguracaoEntidades.Sistema.Sistema, AdicionaSistemaModelo>();
        }
    }
}
