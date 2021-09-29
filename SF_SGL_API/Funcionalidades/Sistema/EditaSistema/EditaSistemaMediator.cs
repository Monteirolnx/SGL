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
        public record EditaSistemaCommand(EditaSistemaInputModel EditaSistemaInputModel) : IRequest;

        public class EditaSistemaCommandHandler : IRequestHandler<EditaSistemaCommand>
        {
            private readonly SGLContexto _sglContexto;

            public EditaSistemaCommandHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }
            public async Task<Unit> Handle(EditaSistemaCommand request, CancellationToken cancellationToken)
            {
                EntidadeSistemaConfig sistema = await _sglContexto.Sistema.FindAsync(new object[] { request.EditaSistemaInputModel.Id }, cancellationToken: cancellationToken);

                sistema.Nome = request.EditaSistemaInputModel.Nome;
                sistema.UrlServicoConsultaLog = request.EditaSistemaInputModel.UsuarioLogin;
                sistema.UsuarioLogin = request.EditaSistemaInputModel.UsuarioLogin;
                sistema.UsuarioSenha = request.EditaSistemaInputModel.UsuarioSenha;

                _sglContexto.Sistema.Update(sistema);
                await _sglContexto.SaveChangesAsync(cancellationToken);

                return default;
            }
        }

        #endregion
    }
}
