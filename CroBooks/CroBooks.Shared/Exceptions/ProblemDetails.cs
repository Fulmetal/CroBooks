namespace CroBooks.Shared.Exceptions
{
    public class ProblemDetails
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string Instance { get; set; } = string.Empty;
        public int Status { get; set; }
        public int SafeDisplayExceptionType { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public List<ExceptionDetail>? ExceptionDetails { get; set; }
        public string TraceId { get; set; } = string.Empty;
    }

    public class ExceptionDetail
    {
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Raw { get; set; } = string.Empty;
        public List<StackFrame>? StackFrames { get; set; }
    }

    public class StackFrame
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public int? Line { get; set; }
        public int? PreContextLine { get; set; }
        public List<string>? PreContextCode { get; set; }
        public List<string>? ContextCode { get; set; }
        public List<string>? PostContextCode { get; set; }
    }
}
