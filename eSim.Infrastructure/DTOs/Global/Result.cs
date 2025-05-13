using System.Text.Json.Serialization;

namespace eSim.Infrastructure.DTOs.Global
{
    public class Result<T>
    {
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
    }
}
