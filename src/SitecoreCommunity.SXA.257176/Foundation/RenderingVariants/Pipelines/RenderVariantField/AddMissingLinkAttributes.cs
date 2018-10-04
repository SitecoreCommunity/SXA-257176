using System;
using System.Linq;
using System.Web.UI;

using Sitecore;
using Sitecore.XA.Foundation.RenderingVariants.Fields;
using Sitecore.XA.Foundation.RenderingVariants.Pipelines.RenderVariantField;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.RenderVariantField;

namespace SitecoreCommunity.SXA.Foundation.RenderingVariants.Pipelines.RenderVariantField
{
    public class AddMissingLinkAttributes : RenderRenderingVariantFieldProcessor
    {
        #region Ignore

        public override void RenderField(RenderVariantFieldArgs args) => throw new NotImplementedException();

        public override Type SupportedType => throw new InvalidOperationException();

        public override RendererMode RendererMode => throw new InvalidOperationException();

        #endregion

        [UsedImplicitly]
        public AddMissingLinkAttributes()
        {
        }

        [UsedImplicitly]
        public new void Process(RenderVariantFieldArgs args)
        {
            if (args.VariantField is VariantSection variantSection)
            {
                if (variantSection.IsLink && string.IsNullOrWhiteSpace(variantSection.Tag))
                {
                    var linkAttributes = variantSection.LinkAttributes;
                    if (linkAttributes.Count > 0)
                    {
                        if (args.ResultControl is RenderFieldControl link)
                        {
                            // RenderFieldControl is not a real control, but dirty hack which consists of
                            // two strings: FirstPart and LastPart, and which is being rendered as
                            // > return FirstPart + RenderChildControls() + LastPart
                            // so we have to use dirty hack to extract href
                            var href = ExtractHref(args.Result);
                            if (string.IsNullOrWhiteSpace(href))
                            {
                                return;
                            }

                            // recreate hyperlink using good-working piece of SXA code
                            var newHyperLink = CreateHyperLink(href, args.Item, false, variantSection.LinkAttributes);

                            // move children
                            MoveChildControls(link, newHyperLink);

                            // save new control and re-render
                            args.ResultControl = newHyperLink;
                            args.Result = RenderControl(newHyperLink);
                        }
                    }
                }
            }
        }

        private static void MoveChildControls(Control source, Control target)
        {
            var childControls = source.Controls
                .OfType<Control>()
                .ToArray();

            foreach (var child in childControls)
            {
                child.Parent.Controls.Remove(child);
                target.Controls.Add(child);
            }
        }

        private string ExtractHref(string html)
        {
            var prefix = "<a href=\"";

            if (!html.StartsWith(prefix))
            {
                return null;
            }

            var start = prefix.Length;
            var closingQuote = html.IndexOf('\"', start);
            var length = closingQuote - start;

            // <a href='bla'> start = 9, closingQuote = 12, length = 12 - 9 = 3
            return html.Substring(start, length);
        }
    }
}