namespace Kuli.Configuration
{
    public sealed class DirectoryDiscoveryOptions
    {
        public const string Section = "dirs";
        
        public string Fragments { get; set; } = "Content";

        public string Templates { get; set; } = "Templates";
        
        public string Output { get; set; } = "Public";
    }
}