using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PlagiarismSystem.Models;

namespace PlagiarismSystem.Infrastructure
{
    public class ConvertFileTagHelper : TagHelper
    { 
        public ConvertResult info {get;set;}
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // TagBuilder result = new TagBuilder("div");
            // foreach(var fm in info.dictionaryFiles)
            // {
            //     foreach(char c in fm.Key)
            //     {
            //         if(c == '\n') 
            //         {
            //             result.InnerHtml.AppendHtml("<br>");
            //             continue;
            //         }
            //         result.InnerHtml.Append(c.ToString());
            //     }
            // }
            // output.Content.AppendHtml(result.InnerHtml);

            // TagBuilder pre = new TagBuilder("pre");
            // foreach(var fm in info.dictionaryFiles)
            // {
            //     pre.InnerHtml.Append(fm.Key);
            // }
            // output.Content.AppendHtml(pre.InnerHtml);


            output.TagName = "pre";
            output.TagMode = TagMode.StartTagAndEndTag;
            string content = "";
            foreach(var fm in info.dictionaryFiles)
            {
                content = fm.Key.ParseContent;
            }
            output.Content.SetHtmlContent(content);
        }
    }
}