namespace Kuli.Configuration
{
    public sealed class DirectoryOptions
    {
        public const string Section = "dirs";

        public string Fragments { get; set; } = "Content";

        public string Templates { get; set; } = "Templates";

        public string Output { get; set; } = "Public";

        public string Assets { get; set; } = "Assets";

        public string AssetsOutput { get; set; } = "Assets";
    }
}