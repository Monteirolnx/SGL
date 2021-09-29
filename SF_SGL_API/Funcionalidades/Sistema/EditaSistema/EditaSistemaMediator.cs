using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL_Infra.ConfiguracaoEntidades.Sistema;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.EditaSistema
{
    public class EditaSistemaMediator
    {
        #region Query

        public record EditaSistemaQuery(int Id) : IRequest<EditaSistemaInputModel>;

        public class EditaSistemaQueryHandler : IRequestHandler<EditaSistemaQuery, EditaSistemaInputModel>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public EditaSistemaQueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public Task<EditaSistemaInputModel> Handle(EditaSistemaQuery request, CancellationToken cancellationToken)
            {
                return _sglContexto.Sistema.Where(s => s.Id == request.Id)
                    .ProjectTo<EditaSistemaInputModel>(_configurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

        #endregion

        #region Command

        public class EditaSistemaCommandHandler : IRequestHandler<EditaSistemaInputModel>
        {
            private readonly SGLContexto _sglContexto;

            public EditaSistemaCommandHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }
            public async Task<Unit> Handle(EditaSistemaInputModel request, CancellationToken cancellationToken)
            {
                EntidadeSistemaConfig sistema = await _sglContexto.Sistema.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

                sistema.Nome = request.Nome;
                sistema.UrlServicoConsultaLog = request.UsuarioLogin;
                sistema.UsuarioLogin = request.UsuarioLogin;
                sistema.UsuarioSenha = request.UsuarioSenha;

                _sglContexto.Sistema.Update(sistema);
                await _sglContexto.SaveChangesAsync(cancellationToken);

                return default;
            }
        }

        #endregion
    }
}
