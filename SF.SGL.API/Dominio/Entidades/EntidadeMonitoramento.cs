namespace SF.SGL.API.Dominio.Entidades;

public class EntidadeMonitoramento
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string Nome { get; set; }

    public string Descricao { get; set; }

    public string Acao { get; set; }

    public string Contato { get; set; }

    public int SistemaId { get; set; }

    [JsonIgnore]
    public EntidadeSistema EntidadeSistema { get; set; }

    public EntidadeMonitoramento(Guid guid, string nome, string descricao, string acao, string contato, int sistemaId)
    {

        Guid = ValidarGuid(guid);
        Nome = ValidarNome(nome);
        Descricao = ValidarDescricao(descricao);
        Acao = ValidarAcao(acao);
        Contato = ValidarContato(contato);
        SistemaId = ValidarSisdemaId(sistemaId);
    }

    private static Guid ValidarGuid(Guid guid)
    {
        return guid;
    }

    private static string ValidarNome(string nome)
    {
        return nome;
    }

    private static string ValidarDescricao(string descricao)
    {
        return descricao;
    }

    private static string ValidarAcao(string acao)
    {
        return acao;
    }

    private static string ValidarContato(string contato)
    {
        DomainExceptionValidation.Quando(!ValidarEmailsContatos(contato), "Lista de contato contém email inválido.");
        return contato;
    }

    private static int ValidarSisdemaId(int sistemaId)
    {
        return sistemaId;
    }

    private static bool ValidarEmailsContatos(string contato)
    {
        bool todosEmailsValidos = false;
        contato = contato.TrimEnd(';');
        string[] emails = contato.Split(';');

        foreach (string email in emails)
        {
            if (email.Trim().EndsWith("."))
            {
                todosEmailsValidos = false;
            }
            try
            {
                MailAddress enderecoEmail = new(email);
                todosEmailsValidos = enderecoEmail.Address == email;
            }
            catch
            {
                todosEmailsValidos = false;
            }
        }
        return todosEmailsValidos;
    }
}
