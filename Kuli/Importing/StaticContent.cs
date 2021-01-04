using System.IO;

namespace Kuli.Importing
{
    public class StaticContent
    {
        public StaticContent(string relativePath, byte[] value)
        {
            Value = value;
            RelativePath = relativePath;
        }

        public string SubDir => Path.GetDirectoryName(RelativePath);

        public string FileName => Path.GetFileNameWithoutExtension(RelativePath);

        public string Extension => Path.GetExtension(RelativePath);

        public string RelativePath { get; }

        public byte[] Value { get; }
    }
}