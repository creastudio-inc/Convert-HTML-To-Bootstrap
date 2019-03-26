using ConvertHTMLToBootstrap.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Windows.Forms;

namespace ConvertHTMLToBootstrap.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
             return View();

        }
 
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}