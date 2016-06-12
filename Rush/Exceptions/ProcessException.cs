using System;

namespace Rush.Exceptions
{
    public class ProcessException : Exception
    {
        public string ErrorTitle { get; set; }
    }
}
