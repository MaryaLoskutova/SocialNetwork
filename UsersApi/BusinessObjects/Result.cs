namespace UsersApi.BusinessObjects
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        private Result(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
        public static Result Ok()
        {
            return new Result(true);
        }        
        
        public static Result Error(string message)
        {
            return new Result(false, message);
        }
    }
}