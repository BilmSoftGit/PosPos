namespace Pospos.Core.Helpers.Jwt
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }

        public T Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }

        
    }
}
