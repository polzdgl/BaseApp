namespace BaseApp.Data.Company.Models
{
    public class CikImportResult
    {
        public List<string> SucceededCiks { get; set; } = new();
        public List<string> FailedCiks { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
