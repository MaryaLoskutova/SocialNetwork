namespace UsersApi.BusinessObjects
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, T value)
        {
            IsSuccess = isSuccess;
            Value = value;
        }

        private Result(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
        public static Result<T> Ok(T value)
        {
            return new Result<T>(true, value);
        }        
        
        public static Result<T> Error(string message)
        {
            return new Result<T>(false, message);
        }
    }
}