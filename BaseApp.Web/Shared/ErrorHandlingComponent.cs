namespace BaseApp.Web.Shared
{
    public static class ErrorHandler
    {
        public static void ResetErrorState(ref bool hasError, ref string? errorMessage)
        {
            hasError = false;
            errorMessage = null;
        }

        public static void HandleError(Exception ex, ref bool hasError, ref string? errorMessage)
        {
            hasError = true;
            errorMessage = ex.Message;
        }
    }
}
