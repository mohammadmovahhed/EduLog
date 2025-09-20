namespace EduLog.CoreLayer.Enums
{
    public enum OperationResultStatus
    {
        Error = 500,        // Internal Server Error
        Success = 200,      // OK
        NotFound = 404,     // Not Found
        BadRequest = 400,   // Bad Request
        Unauthorized = 401  // Unauthorized
    }
}