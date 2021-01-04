using System;

namespace Kuli.Rendering
{
    public class RendererException : Exception
    {
        public RendererException()
        {
        }

        public RendererException(string message) : base(message)
        {
        }

        public RendererException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}