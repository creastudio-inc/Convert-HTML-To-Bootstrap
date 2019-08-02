using HtmlAgilityPack;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class RenderLayout
    {


        public static void Create_Layout(string linkProject)
        {
            var doc = new HtmlWeb().Load(linkProject);
            String Html = "", head = "", header = "", footer = "", script = "";
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//link"))
            {
                head += RenderActionResults.RenderHeader(link);
            }
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//body"))
            {
                foreach (var node in link.ChildNodes)
                {
                    if (node.Name == "header")
                    {
                        header = header_renderHTML(node);
                    }
                    if (node.Name == "div")
                    {
                    }
                    if (node.Name == "footer")
                    {
                        footer = footer_renderHTML(node);
                    }
                    if (node.Name == "script")
                    {
                        script += script_renderHTML(node);
                    }
                }
            }
            Html = Layout_renderHTML(head, header, footer, script);
            FileFolder.File(Property.RacinePathViews + "/Shared/_Layout.cshtml", Html);
        }


        public static String footer_renderHTML(HtmlNode footer)
        {
            foreach (var ChildNode in footer.ChildNodes)
            {
                RenderActionResults.ChildNodes_DIV(ChildNode);
            }
            return footer.OuterHtml + Environment.NewLine;
        }
        public static String header_renderHTML(HtmlNode header)
        {
            foreach (var ChildNode in header.ChildNodes)
            {
                RenderActionResults.ChildNodes_DIV(ChildNode);
            }
            return header.OuterHtml + Environment.NewLine;
        }
        public static String script_renderHTML(HtmlNode script)
        {
            foreach (var att in script.Attributes)
            {
                if (att.Name == "src")
                {
                    Library.Property.ASPScripts.links.Add(att.Value);

                    String linkpath = "";
                    if (att.Value.Contains("//cdnjs") || att.Value.Contains("//code"))
                    {
                        if (att.Value.Contains("https:"))
                        {
                            linkpath = att.Value;
                        }
                        else
                        {
                            linkpath = "http:" + att.Value;
                        }
                    }
                    else
                    {
                        linkpath = Property.RacineURL + "/" + att.Value;
                    }
                    var subpath = att.Value.Split('/');
                    FileFolder.downloadfile(linkpath, "scripts/js/" + subpath[subpath.Length - 1]);
                    att.Value = "~/Content/scripts/js/" + subpath[subpath.Length - 1];
                }
            }
            return script.OuterHtml + Environment.NewLine;
        }


        public static String Layout_renderHTML(String head, String header, String footer, String script)
        {
            String Html = "";
            Html += "<!DOCTYPE html> \n <html>   " + Environment.NewLine;
            Html += Environment.NewLine + "<head>"+ Environment.NewLine;
            Html += Environment.NewLine + head + Environment.NewLine;
            Html += Environment.NewLine + "@RenderSection(\"styles\", required: false)" + Environment.NewLine;
            Html += Environment.NewLine + "</head>" + Environment.NewLine;
            Html += "<body> " + Environment.NewLine;
            Html += header + Environment.NewLine;
            Html += "@RenderBody()" + Environment.NewLine;
            Html += footer + Environment.NewLine;
            Html += script + Environment.NewLine;
            Html += Environment.NewLine + "@RenderSection(\"scripts\", required: false)" + Environment.NewLine;
            Html += "</body>" + Environment.NewLine + "</html>" + Environment.NewLine;
            return Html;

        }


    }
}
