namespace Application.DTOs
{
    public class Result<T>
    {
        public T Data { get; set; }
        public List<string> Messages { get; set; } = new();

        public bool Succeeded { get; set; }


        public static Result<T> Fail()
        {
            return new() { Succeeded = false };
        }

        public static Result<T> Fail(string message)
        {
            return new() { Succeeded = false, Messages = new List<string> { message } };
        }
        public static Result<T> Success(T data)
        {
            return new() { Succeeded = true, Data = data };
        }

        public static Result<T> Success(T data, string message)
        {
            return new() { Succeeded = true, Data = data, Messages = new List<string> { message } };
        }

        public static Result<T> Success(T data, List<string> messages)
        {
            return new() { Succeeded = true, Data = data, Messages = messages };
        }
    }
}
