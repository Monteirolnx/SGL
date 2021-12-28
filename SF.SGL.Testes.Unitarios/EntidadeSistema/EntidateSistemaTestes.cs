namespace SF.SGL.Testes.Unitarios.EntidadeSistema;

public class EntidateSistemaTestes
{
    [Fact(DisplayName = "Criar objeto sistema com estado válido")]
    public void CriarObjEntidadeSistema_ComParametrosValidos_EstadoValido()
    {
        string nomeSistema = "NomeSistema";
        string url = "http://localhost:53463";
        string usuario = "Usuario";
        string senha = "Senha";

        Action action = () =>
        {
            API.Dominio.Entidades.EntidadeSistema entidadeSistema = new(nomeSistema, url, usuario, senha);
        };
        action.Should()
            .NotThrow<DomainExceptionValidation>();
    }


    [Fact(DisplayName = "Criar sistema com nome inválido")]
    public void CriarObjEntidadeSistema_ComParametrosInvalidos_DomainExceptionNomeInvalido()
    {
        string nomeSistemaNulo = string.Empty;
        string nomeSistemaMtoCurto = "L";
        string nomeSistemaMtoLongo = "LoremipsumdolorsitametconsectetueradipiscingelitAeneancommodoligulaegetdolorAeneanmarsitametconsectes";

        string url = "http://localhost:53463";
        string usuario = "Usuario";
        string senha = "Senha";

        Action action = () =>
        {
            API.Dominio.Entidades.EntidadeSistema entidadeSistema = new(nomeSistemaNulo, url, usuario, senha);
        };
        action.Should()
            .Throw<DomainExceptionValidation>();

        action = () =>
        {
            API.Dominio.Entidades.EntidadeSistema entidadeSistema = new(nomeSistemaMtoCurto, url, usuario, senha);
        };
        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Nome de sistema deve ter ao menos 3 caracteres.");

        action = () =>
        {
            API.Dominio.Entidades.EntidadeSistema entidadeSistema = new(nomeSistemaMtoLongo, url, usuario, senha);
        };
        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Nome de sistema não deve ter mais do que 100 caracteres.");

    }


    [Fact(DisplayName = "Criar sistema com url de serviço de consulta de log inválida.")]
    public void CriarObjEntidadeSistema_ComParametrosInvalidos_DomainExceptionUrlInvalida()
    {
        string nomeSistema = "NomeSistema";
        string usuario = "Usuario";
        string senha = "Senha";

        string testeUrlNula = string.Empty;
        string testeUrlInvalida = "StringInvalida";


        Action action = () =>
        {
            API.Dominio.Entidades.EntidadeSistema entidadeSistema = new(nomeSistema, testeUrlNula, usuario, senha);
        };
        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Url de serviço de consulta de log do sistema inválida.");

        action = () =>
        {
            API.Dominio.Entidades.EntidadeSistema entidadeSistema = new(nomeSistema, testeUrlInvalida, usuario, senha);
        };
        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Url de serviço de consulta de log do sistema inválida.");
    }

}
