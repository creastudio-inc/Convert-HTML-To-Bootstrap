using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library
{
    public class RenderControllers
    {

        public string GetUsing()
        {

            return "using System; \n" +
                   "using System.Collections.Generic;\n" +
                    "using System.Linq;\n" +
                    "using System.Web;\n" +
                    "using System.Web.Mvc; \n";
        }

        public string GetNamespace()
        {
            return "namespace "+ Property.ASPModel.NameSpace+ ".Controllers";
        }
        public string GetControllers(String Name)
        {
            return "\t public class "+ Name + "Controller : Controller";
        }
        public string Open()
        {
            return "{ \n";
        }
        public string Close()
        {
            return "} \n";
        }

        public string GetActionResults(List<ASPActionResult> actionResults)
        {
            var stringresult = "";
            foreach(var item in actionResults)
            {
                stringresult += "\n";
                stringresult += "\t public ActionResult " + item.Name+ "() \n";
                stringresult += "\t \t {\n";
                stringresult += "\t \t \t return View();\n";
                stringresult += "\t \t }\n";
                stringresult += "\n";
            }
            return stringresult;
        }
    }
}
