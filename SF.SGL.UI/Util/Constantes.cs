namespace SF.SGL.UI.Util
{
    public class Constantes
    {
        public static class ClaimsTypes
        {

            public const string LoginDeRede = "preferred_username";
            public const string Email = "email";
            public const string Foto = "thumbnailPhoto";
            public const string Lotacao = "department";
            public const string Telefone = "phone_number";
            public const string Role = "role";
            public const string Name = "name";
        }

        public static class Roles
        {
            public const string UnidadesCreate = "unidades.create";
            public const string UnidadesRead = "unidades.read";
            public const string UnidadesUpdate = "unidades.update";
            public const string UnidadesDelete = "unidades.delete";
        }

        public static class LocalStorageKey
        {
            public const string Theme = "theme";
            public const string Counter = "counter";
        }

        public static class HttpClients
        {
            public const string Server = "server";
        }
    }
}
