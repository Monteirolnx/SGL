using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistema.ExisteSistema
{
    public class ExisteSistemaMediator
    {
        #region Query

        public record ExisteSistemaQuery(int Id) : IRequest<bool>;

        public class ExisteSistemaQueryHandler : IRequestHandler<ExisteSistemaQuery, bool>
        {
            private readonly SGLContexto _sglContexto;

            public ExisteSistemaQueryHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }
            public Task<bool> Handle(ExisteSistemaQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_sglContexto.Sistema.Where(s => s.Id == request.Id).Any());
            }
        }

        #endregion
    }
}
