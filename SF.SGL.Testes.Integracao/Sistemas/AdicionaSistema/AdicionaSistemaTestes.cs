using System.Threading.Tasks;
using SF.SGL.API.Funcionalidades.Sistemas;
using SF.SGL.Dominio.Entidades;
using Shouldly;
using Xunit;

namespace SF.SGL.Testes.Integracao.Sistemas
{
    [Collection(nameof(SGLFixture))]
    public class AdicionaSistemaTestes
    {
        private readonly SGLFixture _sglFixture;

        public AdicionaSistemaTestes(SGLFixture dispositivoTestes)
        {
            _sglFixture = dispositivoTestes;
        }

        [Fact]
        public async Task Deve_Criar_Sistema()
        {
            AdicionaSistema.Command command = new()
            {
                Nome = "Sistema Teste",
                UrlServicoConsultaLog = "Url",
                UsuarioLogin = "UsuarioLogin",
                UsuarioSenha = "UsuarioSenha"
            };

            int sistemaId = await _sglFixture.SendAsync(command);
            EntidadeSistema sistema = await _sglFixture.FindAsync<EntidadeSistema>(sistemaId);

            sistema.ShouldNotBeNull();
            sistema.Nome.ShouldBe(command.Nome);
            sistema.UrlServicoConsultaLog.ShouldBe(command.UrlServicoConsultaLog);
            sistema.UsuarioLogin.ShouldBe(command.UsuarioLogin);
            sistema.UsuarioSenha.ShouldBe(command.UsuarioSenha);
        }
    }
}
