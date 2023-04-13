using System;
using System.Linq;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace INTEX.Infrastructure
{

    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PaginationTagHelper : TagHelper
    {
        //Dynamically create the page links

        private IUrlHelperFactory uhf;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext Vc { get; set; }

        // Different from View Context
        public PageInfo PageModel { get; set; }
        public string PageAction { get; set; }
        public string PageClass { get; set; }
        public bool PageClassEnabled { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(Vc);

            TagBuilder final = new TagBuilder("div");

            TagBuilder prev = new TagBuilder("a");
            if (PageModel.CurrentPage > 1)
            {
                prev.Attributes["href"] = uh.Action(PageAction, new { pageNum = PageModel.CurrentPage - 1 });
            }
            prev.AddCssClass(PageClass);
            prev.InnerHtml.Append("<< Prev ");

            final.InnerHtml.AppendHtml(prev);


            int startPage, endPage;
            if (PageModel.CurrentPage <= 2)
            {
                startPage = 1;
                endPage = Math.Min(PageModel.TotalPages, 4);
            }
            else if (PageModel.CurrentPage >= PageModel.TotalPages - 1)
            {
                startPage = Math.Max(1, PageModel.TotalPages - 3);
                endPage = PageModel.TotalPages;
            }
            else
            {
                startPage = PageModel.CurrentPage - 1;
                endPage = PageModel.CurrentPage + 2;
            }

            for (int i = startPage; i <= endPage; i++)
            {
                TagBuilder tb = new TagBuilder("a");
                tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i });

                if (PageClassEnabled)
                {
                    tb.AddCssClass(PageClass);
                    tb.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }
                tb.InnerHtml.Append(i.ToString());

                final.InnerHtml.AppendHtml(tb);
            }

            TagBuilder next = new TagBuilder("a");
            next.Attributes["href"] = uh.Action(PageAction, new { pageNum = PageModel.CurrentPage + 1 });
            next.AddCssClass(PageClass);
            next.InnerHtml.Append(" Next >>");

            final.InnerHtml.AppendHtml(next);

            tho.Content.AppendHtml(final.InnerHtml);
        }

    }
}
