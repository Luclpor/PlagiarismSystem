using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PlagiarismSystem.HtmlHelpers
{
    public static class TreeHelper
    {
        // public static HtmlString CreateTree(this IHtmlHelper html)
        // {
        //     TagBuilder ul = new TagBuilder("ul");
        //     ul.Attributes.Add("class","tree-padding tree-vertical-lines tree-horizontal-lines tree-summaries tree-markers tree-buttons");
        //     TagBuilder li = new TagBuilder("li");
        //     ul.InnerHtml.AppendHtml(li);
        //     TagBuilder details = new TagBuilder("details");
        //     details.Attributes.Add("open","true");
        //     li.InnerHtml.AppendHtml(details);
        //     TagBuilder summary = new TagBuilder("summary");
        //     summary.InnerHtml.Append("Папки на сервере");
        //     details.InnerHtml.AppendHtml(summary);
        //     TagBuilder childUl = new TagBuilder("ul");
        //     childUl.Attributes.Add("id", "folders_list");
        //     details.InnerHtml.AppendHtml(childUl);
        //     using var writer = new StringWriter();
        //     ul.WriteTo(writer,HtmlEncoder.Default);
        //     return new HtmlString(writer.ToString());
        // }
        public static HtmlString CreateTree(this IHtmlHelper html)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.Attributes.Add("class","list-group");
            TagBuilder li = new TagBuilder("li");
            li.Attributes.Add("class","list-group-item");
            ul.InnerHtml.AppendHtml(li);
            TagBuilder details = new TagBuilder("details");
            details.Attributes.Add("open","true");
            li.InnerHtml.AppendHtml(details);
            TagBuilder summary = new TagBuilder("summary");
            summary.InnerHtml.Append("Папки на сервере");
            details.InnerHtml.AppendHtml(summary);
            TagBuilder childUl = new TagBuilder("ul");
            childUl.Attributes.Add("id", "folders_list");
            childUl.Attributes.Add("class", "list-group list-group-flush");
            details.InnerHtml.AppendHtml(childUl);
            using var writer = new StringWriter();
            ul.WriteTo(writer,HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}