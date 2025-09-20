using EduLog.CoreLayer.Enums;

namespace EduLog.CoreLayer.Utilities
{
    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }
    }

    public class OperationResult
    {
        public string? Message { get; set; }
        public OperationResultStatus Status { get; set; }
        public bool IsSuccess => Status == OperationResultStatus.Success;

        #region Factory Methods
        public static OperationResult Error(string? message = null)
        {
            return new OperationResult()
            {
                Status = OperationResultStatus.Error,
                Message = message ?? "عملیات ناموفق بود",
            };
        }

        public static OperationResult NotFound(string? message = null)
        {
            return new OperationResult()
            {
                Status = OperationResultStatus.NotFound,
                Message = message ?? "اطلاعات درخواستی یافت نشد",
            };
        }

        public static OperationResult Success(string? message = null)
        {
            return new OperationResult()
            {
                Status = OperationResultStatus.Success,
                Message = message ?? "عملیات با موفقیت انجام شد",
            };
        }
        #endregion

        #region Generic Version (Optional)
        public static OperationResult<T> Success<T>(T data, string? message = null)
        {
            return new OperationResult<T>()
            {
                Status = OperationResultStatus.Success,
                Message = message ?? "عملیات با موفقیت انجام شد",
                Data = data
            };
        }

        public static OperationResult<T> Error<T>(OperationResultStatus? status = null,
                                                  string? message = null)
        {
            return new OperationResult<T>()
            {
                Status = status?? OperationResultStatus.Error,
                Message = message ?? "عملیات ناموفق بود",
                Data = default
            };
        }

        #endregion
    }
}