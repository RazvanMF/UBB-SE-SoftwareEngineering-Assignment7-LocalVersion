namespace NamespaceGPT.Common.ConfigurationManager.Module.JsonManager.JsonParser
{
    public enum JsonTokenType
    {
        String,
        Number,

        True,
        False,

        Null,

        ObjectStart,
        ObjectEnd,

        ArrayStart,
        ArrayEnd,

        Colon,
        Comma
    }
}
