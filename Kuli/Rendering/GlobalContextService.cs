using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fluid;
using Kuli.Importing;
using Microsoft.Extensions.Configuration;

namespace Kuli.Rendering
{
    public class GlobalContextService
    {
        public GlobalContextService(IConfiguration configuration)
        {
            Categories = new Dictionary<string, Fragment[]>();
            Fragments = new Dictionary<string, Fragment>();
            Templates = new Dictionary<string, FluidTemplate>();
            Items = new Dictionary<string, string>();
            
            configuration.GetSection("site").Bind(Items);
        }

        public IDictionary<string, Fragment[]> Categories { get; }
        
        public IDictionary<string, Fragment> Fragments { get; }
        
        public IDictionary<string, FluidTemplate> Templates { get; }
        
        public IDictionary<string, string> Items { get; }

        public void ApplyValues(TemplateContext ctx)
        {
            ctx.SetValue("Categories", Categories);
            ctx.SetValue("Fragments", Fragments);
            ctx.SetValue("Templates", Templates);
            ctx.SetValue("Items", Items);
        }
    }
}