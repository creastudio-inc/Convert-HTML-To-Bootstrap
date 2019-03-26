using ConvertHTMLToBootstrap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConvertHTMLToBootstrap.Controllers
{
    public class ConvertController : Controller
    {
        // GET: Convert
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ASP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ASP(Library.Models.ASPModel model)
        {
            Library.Property.ASPModel = model;
            Library.Property.RacineURL = model.LinkProject; 
            Library.Property.RacinePathContent = Server.MapPath("Content"); 
            Library.Main Convert = new Library.Main();
            // create Folder 
            Convert.Create_Folder();
            // create folder  controller & ActionResult & css & js  
            Convert.Create_Folder_Controller();
            Convert.Create_Folder_ActionResult();
            Convert.Create_Folder_Css();
            Convert.Create_Folder_Js();
            return View();
        }

        public ActionResult ASP2(string id)
        {
            ViewBag.ID = id;
            return View();
        }

        public ActionResult ASP3(string id, String Name, String Type, String Link, String count)
        {
            ViewBag.ID = id;
            ViewBag.Name = Name;
            ViewBag.Type = Type;
            ViewBag.Link = Link;
            ViewBag.count = count;
            return View();
        }
    }
}