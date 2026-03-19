namespace AFCS.API.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        // ✓ 200 — return data successfully
        public static Result<T> Ok(T data) => new()
        {
            Success = true,
            StatusCode = 200,
            Data = data
        };

        // ✓ 201 — created successfully
        public static Result<T> Created(T data) => new()
        {
            Success = true,
            StatusCode = 201,
            Data = data
        };

        // ✗ 404 — not found
        public static Result<T> NotFound(List<string> error) => new()
        {
            Success = false,
            StatusCode = 404,
            Errors = error
        };

        // ✗ 400 — bad request / validation failed
        public static Result<T> BadRequest(List<string> error) => new()
        {
            Success = false,
            StatusCode = 400,
            Errors = error
        };

        // ✗ 500 — something crashed on the server
        public static Result<T> ServerError(List<string> error) => new()
        {
            Success = false,
            StatusCode = 500,
            Errors = error
        };
    }
}
