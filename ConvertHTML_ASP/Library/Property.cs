using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class Property
    {
        public static List<string> AllLinksPages = new List<string>();
        public static String RacineURL = "";
        public static String RacinePathViews = String.Empty;
        public static String RacinePathControllers = String.Empty;
        public static String RacinePathContent = String.Empty;
        public static Boolean IsFinish = false;
        public static ASPModel ASPModel;
    }
}
