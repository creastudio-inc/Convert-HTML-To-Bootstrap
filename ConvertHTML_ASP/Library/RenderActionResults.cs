using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Library.Models;

namespace Library
{
    public static class RenderActionResults
    {
        public static void Create_View(String controllerName,List<ASPActionResult> actionResults)
        {
            foreach (var actionResult in actionResults)
            {               
                string contentscss = GetPageCodeCss(actionResult.Link);
                string contentsJs = GetPageCodeJs(actionResult.Link);
                string contents = GetPageCode(actionResult.Link);
                string codeHtmlFinal = "";
                codeHtmlFinal += Environment.NewLine + "{" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + "@ViewBag.Title ='"+actionResult.Name+"';" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + "}" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + "@section styles{" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + contentscss + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + "}" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + "@section scripts{" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + contentsJs + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + "}" + Environment.NewLine;
                codeHtmlFinal += Environment.NewLine + contents + Environment.NewLine;

                FileFolder.File(Property.RacinePathViews + "/"+ controllerName+"/" + actionResult.Name+".cshtml", codeHtmlFinal);
            }
        }

        private static string GetPageCodeCss(string link)
        {
            List<HtmlNode> HtmlNode = new List<HtmlNode>();
            var doclink = new HtmlWeb().Load(link);
            string head = "";
            foreach (HtmlNode linkscript in doclink.DocumentNode.SelectNodes("//link"))
            {
                head += RenderActionResults.RenderHeaderStyle(linkscript);
            }
            return head;
        }
        private static string GetPageCodeJs(string link)
        {
            var doclink = new HtmlWeb().Load(link);

            string script = "";
            foreach (HtmlNode linkscript in doclink.DocumentNode.SelectNodes("//script"))
            {
                script += RenderActionResults.RenderHeaderScript(linkscript);
            }
            return script;

        }

        private static string GetPageCode(string link)
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
            var doclink = new HtmlWeb().Load(link);
          
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
                
            }
            return HtmlNode;
        }

        private static string RenderHeaderScript(HtmlNode node)
        {
            String script = "";

            if (node.Name == "script")
            {
                var attjs = node.Attributes.Where(x => x.Name == "src").FirstOrDefault();
                if (attjs != null)
                {
                    if (!Library.Property.ASPScripts.links.Contains(attjs.Value))
                    {
                        String linkpath = "";
                        if (attjs.Value.Contains("//cdnjs") || attjs.Value.Contains("//code"))
                        {
                            if (attjs.Value.Contains("https:"))
                            {
                                linkpath = attjs.Value;
                            }
                            else
                            {
                                linkpath = "http:" + attjs.Value;
                            }
                        }
                        else
                        {
                            linkpath = Property.RacineURL + "/" + attjs.Value;
                        }
                        var subpath = attjs.Value.Split('/');
                        FileFolder.downloadfile(linkpath, "scripts/js/" + subpath[subpath.Length - 1]);
                        attjs.Value = "~/Content/scripts/js/" + subpath[subpath.Length - 1];
                        script += node.OuterHtml + Environment.NewLine;

                    }

                }

            }

            return script;
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
                                    //att.Value = "@Url.Action(\"index\",\"" + att.Value.Replace(".html", "") + "\")";
                                    att.Value = "#";
                                }
                            }
                            if ( !ChildNode.InnerHtml.Contains("<svg") && !ChildNode.InnerHtml.Contains("<img") && !ChildNode.InnerHtml.Contains("<span") && !ChildNode.InnerHtml.Contains("<div"))
                                ChildNode.InnerHtml =  ChildNode.InnerHtml.Replace("\n", "").Trim();

                            break;

                        case "#text":
                            if (!ChildNode.InnerHtml.Contains("\n") )
                                ChildNode.InnerHtml =  ChildNode.InnerHtml.Replace("\n", "").Trim() ;

                            break;

                        case "p":

                            if (ChildNode.ChildNodes.Count == 1 && ChildNode.ChildNodes[0].Name == "#text"  && ChildNode.InnerHtml != " ")
                                ChildNode.InnerHtml = ChildNode.InnerHtml.Replace("\n", "").Trim() ;

                            break;

                        case "div":
                            if (ChildNode.ChildNodes.Count == 1 && ChildNode.ChildNodes[0].Name == "#text"  && ChildNode.InnerHtml != " ")
                                ChildNode.InnerHtml =  ChildNode.InnerHtml.Replace("\n", "").Trim() ;

                            break;

                        case "span":
                            if ( !ChildNode.InnerHtml.Contains("<i>"))
                                ChildNode.InnerHtml =  ChildNode.InnerHtml.Replace("\n", "").Trim() ;

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
                                    if (!att.Value.Contains("\n"))
                                        att.Value =  att.Value.Replace("\n", "").Trim() ;
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




        public static String RenderHeader(HtmlNode node)
        {
            String head = "";
             
                if (node.Name == "link")
                {
                var attcss = node.Attributes.Where(x => x.Name == "rel" && x.Value == "stylesheet").FirstOrDefault();
                if (attcss != null) {
                    var att = node.Attributes.Where(x => x.Name == "href").FirstOrDefault();
                    if (att.Name == "href"  )
                        {
                        Library.Property.ASPStyles.links.Add(att.Value);

                        String linkpath = "";
                            if (att.Value.Contains("//code.jquery.com"))
                            {
                                linkpath = "http:" + att.Value;
                            }
                            else 
                            if (att.Value.Contains("https://fonts.")){
                                linkpath =  att.Value;
                                att.Value = linkpath;
                        }
                        else
                            {
                            linkpath = Property.RacineURL+"/" + att.Value;
                            var subpath = att.Value.Split('/');
                            FileFolder.downloadfile(linkpath, "css/" + subpath[subpath.Length - 1]);
                            att.Value = "~/Content/css/" + subpath[subpath.Length - 1];

                        }

                    }
                }

                    head += node.OuterHtml + Environment.NewLine;
                }
             
            return head;
        }

        
        public static String RenderHeaderStyle(HtmlNode node)
        {
            String head = "";
             
                if (node.Name == "link")
                {
                var attcss = node.Attributes.Where(x => x.Name == "rel" && x.Value == "stylesheet").FirstOrDefault();
                if (attcss != null) {
                    var att = node.Attributes.Where(x => x.Name == "href").FirstOrDefault();
                    if (!Library.Property.ASPStyles.links.Contains(att.Value))
                    {
                        if (att.Name == "href")
                        {
                            String linkpath = "";
                            if (att.Value.Contains("//code.jquery.com"))
                            {
                                linkpath = "http:" + att.Value;
                            }
                            else
                            if (att.Value.Contains("https://fonts."))
                            {
                                linkpath = att.Value;
                                att.Value = linkpath;
                            }
                            else
                            {
                                linkpath = Property.RacineURL + "/" + att.Value;
                                var subpath = att.Value.Split('/');
                                FileFolder.downloadfile(linkpath, "css/" + subpath[subpath.Length - 1]);
                                att.Value = "~/Content/css/" + subpath[subpath.Length - 1];

                            }
                            head += node.OuterHtml + Environment.NewLine;

                        }
                    }
                  
                }

                }
             
            return head;
        }


    }
}
