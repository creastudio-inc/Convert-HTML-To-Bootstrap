using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class ASPModel
    {
        public String NameProject { get; set; }
        public String NameSpace { get; set; }
        public String LinkProject { get; set; }
        public String blankPage { get; set; }
        public String NameTranslate { get; set; }
        public String DatabaseString { get; set; }
        public List<ASPController> Controllers { get; set; }
    }
}
