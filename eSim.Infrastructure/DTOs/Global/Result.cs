using System.Text.Json.Serialization;

namespace eSim.Infrastructure.DTOs.Global
{
    public class Result<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "success";

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
        public int StatusCode { get; set; }
    }
}
