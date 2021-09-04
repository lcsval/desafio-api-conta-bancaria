using System.Collections.Generic;

namespace Desafio.Domain
{
    public class Result<T>
    {
        public Result(bool succeeded, T data, IEnumerable<string> errors = null)
        {
            Succeeded = succeeded;
            Data = data;
            Errors = errors;
        }

        public bool Succeeded { get; }

        public T Data { get; }

        public IEnumerable<string> Errors { get; }

        public static Result<T> Success()
        {
            return new Result<T>(true, default(T), new string[] { });
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, new string[] { });
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, default(T), errors);
        }

        public static Result<T> Failure(IReadOnlyCollection<string> notifications)
        {
            return new Result<T>(false, default(T), notifications);
        }
    }
}
