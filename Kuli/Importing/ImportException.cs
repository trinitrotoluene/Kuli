using System;

namespace Kuli.Importing
{
    public class ImportException : Exception
    {
        public ImportException()
        {
        }

        public ImportException(string message) : base(message)
        {
        }

        public ImportException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}