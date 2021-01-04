using System.Collections.Generic;

namespace Kuli.Importing
{
    public class Fragment
    {
        public Fragment(string name, Dictionary<string, string> frontMatter, string html)
        {
            Name = name;
            FrontMatter = frontMatter;
            Html = html;
        }

        public string Name { get; }
        
        public Dictionary<string, string> FrontMatter { get; }
        
        public string Html { get; }

        public string Template => FrontMatter.TryGetValue("template", out var template) ? template : "default";

        public string Type => FrontMatter.TryGetValue("type", out var type) ? type : "fragment";
    }
}