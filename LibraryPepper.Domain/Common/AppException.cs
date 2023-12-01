namespace ISF.FAF_RF.Domain.Common
{
    public class AppException : Exception
    {
        public AppError Error { get; private set; }

        public AppException(AppError error, string? message = null)
            : base($"{error.Message}\n{message}")
        {
            Error = error;
        }
    }
}
