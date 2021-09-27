using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_SGL.Entidades;
using Shouldly;
using Xunit;
using static SF_SGL.Features.Sistema.AdicionaSistema;

namespace SF_SGL.IntegrationTests.Pages.Sistemas.Adiciona
{
    [Collection(nameof(SliceFixture))]
    public class Testes
    {
        private readonly SliceFixture _fixture;

        public Testes(SliceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Deve_Criar_Sistema()
        {
            Command cmd = new()
            {
                Nome = "Sistema Controle Teste Integração",
                UrlServicoConsultaLog = "https://localhost:44347/",
                UsuarioLogin = "UsuarioControle",
                UsuarioSenha = "UsuarioSenhaControle"
            };

            int sistemaId = await _fixture.SendAsync(cmd);

            Sistema sistema = await _fixture.FindAsync<Sistema>(sistemaId);

            sistema.ShouldNotBeNull();
            sistema.Nome.ShouldBe(cmd.Nome);
            sistema.UrlServicoConsultaLog.ShouldBe(cmd.UrlServicoConsultaLog);
            sistema.UsuarioLogin.ShouldBe(cmd.UsuarioLogin);
            sistema.UsuarioSenha.ShouldBe(cmd.UsuarioSenha);
        }
    }
}
