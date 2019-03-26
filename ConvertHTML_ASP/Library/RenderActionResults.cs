using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Library.Models;

namespace Library
{
    public class RenderActionResults
    {
        internal void Create_View(String controllerName,List<ASPActionResult> actionResults)
        {
            foreach (var actionResult in actionResults)
            {               
                string contents = GetPageCode(actionResult.Link);
                FileFolder.File(Property.RacinePathViews + "/"+ controllerName+"/" + actionResult.Name+".cshtml", contents);
            }
        }

        private string GetPageCode(string link)
        {
            var HtmlNodes = GetHtmlNode(link);

            String contentsview = "";
            foreach (var Node in HtmlNodes)
            {
                foreach (var ChildNode in Node.ChildNodes)
                {
                    ChildNodes_DIV(ChildNode);
                }
                contentsview += Node.OuterHtml;
            }
            return contentsview;
        }
        public static List<HtmlNode> GetHtmlNode(string link)
        {
            List<HtmlNode> HtmlNode = new List<HtmlNode>();
            var doclink = new HtmlWeb().Load(Property.RacineURL + link);
            var bodyNode = doclink.DocumentNode.SelectNodes("//body");
            foreach (HtmlNode body in bodyNode)
            {
                foreach (var node in body.ChildNodes)
                {
                    if (node.Name == "header" || node.Name == "#text" || node.Name == "footer" || node.Name == "script")
                    {
                    }
                    else
                    {
                        HtmlNode.Add(node);
                    }
                }
                HtmlNode.RemoveAt(0);
            }
            return HtmlNode;
        }

        public static void ChildNodes_DIV(HtmlNode footer)
        {
            if (footer.ChildNodes.Count > 0)
            {
                foreach (var ChildNode in footer.ChildNodes)
                {
                    switch (ChildNode.Name)
                    {
                        case "img":
                            foreach (var att in ChildNode.Attributes)
                            {
                                if (att.Name == "src")
                                {
                                    String linkpath = "";
                                    if (att.Value.Contains("//cdnjs") || att.Value.Contains("//code"))
                                    {
                                        linkpath = "http:" + att.Value;
                                    }
                                    else
                                    {
                                        linkpath = Property.ASPModel.LinkProject + att.Value;
                                    }
                                    //FileFolder.downloadfile(linkpath, att.Value);
                                    att.Value = "~/Content/" + att.Value;
                                }
                            }
                            break;

                        case "a":

                            foreach (var att in ChildNode.Attributes)
                            {
                                if (att.Name == "href" && att.Value.Contains(".html"))
                                {
                                    att.Value = "@Url.Action(\"index\",\"" + att.Value.Replace(".html", "") + "\")";
                                }
                            }
                            if (!ChildNode.InnerHtml.Contains("@Html.CustomDisplayText") && !ChildNode.InnerHtml.Contains("<svg") && !ChildNode.InnerHtml.Contains("<img") && !ChildNode.InnerHtml.Contains("<span") && !ChildNode.InnerHtml.Contains("<div"))
                                ChildNode.InnerHtml = "@Html.CustomDisplayText(\"" + ChildNode.InnerHtml.Replace("\n", "").Trim() + " \")";

                            break;

                        case "#text":
                            if (!ChildNode.InnerHtml.Contains("\n") && !ChildNode.InnerHtml.Contains("@Html.CustomDisplayText"))
                                ChildNode.InnerHtml = "@Html.CustomDisplayText(\"" + ChildNode.InnerHtml.Replace("\n", "").Trim() + " \")";

                            break;

                        case "p":

                            if (ChildNode.ChildNodes.Count == 1 && ChildNode.ChildNodes[0].Name == "#text" && !ChildNode.InnerHtml.Contains("@Html.CustomDisplayText") && ChildNode.InnerHtml != " ")
                                ChildNode.InnerHtml = "@Html.CustomDisplayText(\"" + ChildNode.InnerHtml.Replace("\n", "").Trim() + " \")";

                            break;

                        case "div":
                            if (ChildNode.ChildNodes.Count == 1 && ChildNode.ChildNodes[0].Name == "#text" && !ChildNode.InnerHtml.Contains("@Html.CustomDisplayText") && ChildNode.InnerHtml != " ")
                                ChildNode.InnerHtml = "@Html.CustomDisplayText(\"" + ChildNode.InnerHtml.Replace("\n", "").Trim() + " \")";

                            break;

                        case "span":
                            if (!ChildNode.InnerHtml.Contains("@Html.CustomDisplayText") && !ChildNode.InnerHtml.Contains("<i>"))
                                ChildNode.InnerHtml = "@Html.CustomDisplayText(\"" + ChildNode.InnerHtml.Replace("\n", "").Trim() + " \")";

                            break;

                        case "input":
                            foreach (var att in ChildNode.Attributes)
                            {
                                if (att.Name == "pattern")
                                {
                                    att.Value = "";
                                }
                                if (att.Name == "placeholder")
                                {
                                    if (!att.Value.Contains("\n") && !att.Value.Contains("@Html.CustomDisplayText"))
                                        att.Value = "@Html.CustomDisplayText(\"" + att.Value.Replace("\n", "").Trim() + " \")";
                                }
                            }
                            break;
                    }

                    if (ChildNode.ChildNodes.Count > 0)
                    {
                        ChildNodes_DIV(ChildNode);
                    }
                }
            }
        }


        public void Create_Layout(string linkProject)
        {
            var doc = new HtmlWeb().Load(linkProject);
            String Html = "", head = "", header = "", footer = "", script = "";
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//head"))
            {
                head = RenderHeader(link);
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
                ChildNodes_DIV(ChildNode);
            }
            return footer.OuterHtml + Environment.NewLine;
        }

        public static String header_renderHTML(HtmlNode header)
        {
            foreach (var ChildNode in header.ChildNodes)
            {
                ChildNodes_DIV(ChildNode);
            }
            return header.OuterHtml + Environment.NewLine;
        }

        public static String Layout_renderHTML(String head, String header, String footer, String script)
        {
            String Html = "";
            Html += "<!DOCTYPE html> \n <html>   " + Environment.NewLine;
            Html += Environment.NewLine + "" + head + Environment.NewLine;
            Html += "<body> " + Environment.NewLine;
            Html += header + Environment.NewLine;
            Html += "@RenderBody()" + Environment.NewLine;
            Html += footer + Environment.NewLine;
            Html += script + Environment.NewLine;
            Html += "</body>" + Environment.NewLine + "</html>" + Environment.NewLine;
            return Html;

        }
        public static String RenderHeader(HtmlNode node)
        {
            String head = "";
             
                if (node.Name == "link")
                {
                    foreach (var att in node.Attributes)
                    {
                        if (att.Name == "href")
                        {
                            String linkpath = "";
                            if (att.Value.Contains("//code.jquery.com"))
                            {
                                linkpath = "http:" + att.Value;
                            }
                            else
                            {
                                linkpath = Property.RacineURL + att.Value;
                            }
                            var subpath = att.Value.Split('/');
                            FileFolder.downloadfile(linkpath, "css/" + subpath[subpath.Length - 1]);
                            att.Value = "~/Content/css/" + subpath[subpath.Length - 1];

                        }
                    }

                    head += node.OuterHtml + Environment.NewLine;
                }
             
            return head;
        }

        public static String script_renderHTML(HtmlNode script)
        {
            foreach (var att in script.Attributes)
            {
                if (att.Name == "src")
                {
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
                        linkpath = Property.RacinePathContent + att.Value;
                    }
                    var subpath = att.Value.Split('/');
                    FileFolder.downloadfile(linkpath, "scripts/js/" + subpath[subpath.Length - 1]);
                    att.Value = "~/Content/scripts/js/" + subpath[subpath.Length - 1];
                }
            }
            return script.OuterHtml + Environment.NewLine;
        }

    }
}
