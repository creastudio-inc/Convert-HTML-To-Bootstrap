using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConvertHTMLToBootstrap.Models
{
    public class ASPModel
    {
       public String NameProject { get; set; }
       public String NameSpace { get; set; }
       public String LinkProject { get; set; }
       public String NameTranslate { get; set; }
       public String DatabaseString { get; set; }
        public List<ASPController> Controllers { get; set; }
    }

    public class ASPController
    {
        public String Name { get; set; }

        public List<ASPActionResult> actionResults { get; set; }
    }

    public class ASPActionResult
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public String Link { get; set; }
    }
}