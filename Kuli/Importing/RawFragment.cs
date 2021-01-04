namespace Kuli.Importing
{
    public class RawFragment
    {
        public RawFragment(string frontMatter, string pageMarkdown, string fileName)
        {
            FrontMatter = frontMatter;
            PageMarkdown = pageMarkdown;
            FileName = fileName;
        }

        public string FrontMatter { get; }

        public string PageMarkdown { get; }

        public string FileName { get; set; }
    }
}