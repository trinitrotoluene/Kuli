using System.Collections.Generic;
using Fluid;
using Kuli.Importing;
using Microsoft.Extensions.Configuration;

namespace Kuli.Rendering
{
    public class SiteRenderingContext
    {
        public SiteRenderingContext(IConfiguration configuration)
        {
            StaticContent = new Dictionary<string, StaticContent>();
            Fragments = new Dictionary<string, Fragment>();
            Templates = new Dictionary<string, FluidTemplate>();
            Items = new Dictionary<string, string>();

            configuration.GetSection("site").Bind(Items);
        }

        public IDictionary<string, Fragment> Fragments { get; }

        public IDictionary<string, FluidTemplate> Templates { get; }

        public IDictionary<string, string> Items { get; }

        public IDictionary<string, StaticContent> StaticContent { get; }

        public void ApplyValues(TemplateContext ctx)
        {
            ctx.SetValue("Fragments", Fragments.Values);
            ctx.SetValue("Templates", Templates.Values);
            ctx.SetValue("Items", Items);
        }

        static SiteRenderingContext()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<Fragment>();
        }
    }
}