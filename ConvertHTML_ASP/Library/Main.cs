using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Main
    {
        public void Create_Folder()
        {
            FileFolder.Folder(Property.RacinePathContent + "/" + Property.ASPModel.NameSpace);
        }
  
        public void Create_Layout()
        {
            var urlViews = Property.RacinePathContent + "/" + Property.ASPModel.NameProject + "/Views";
            Property.RacinePathViews = urlViews;
            FileFolder.Folder(urlViews);
            FileFolder.Folder(urlViews + "/Shared");
            RenderLayout.Create_Layout(Property.ASPModel.LinkProject);
        }
  
        public void Create_Folder_ActionResult()
        {
            var urlViews = Property.RacinePathContent + "/" + Property.ASPModel.NameProject + "/Views";
            Property.RacinePathViews = urlViews;
            FileFolder.Folder(urlViews);
            foreach (var controller in Property.ASPModel.Controllers)
            {
                FileFolder.Folder(urlViews+"/"+controller.Name);
                RenderActionResults.Create_View(controller.Name,controller.actionResults);
            }
        }

        public void Create_Folder_Controller()
        {
            var urlcontroller = Property.RacinePathContent + "/" + Property.ASPModel.NameProject + "/Controllers";
            FileFolder.Folder(urlcontroller);
            Property.RacinePathControllers = urlcontroller;

            RenderControllers renderControllers = new RenderControllers();
            foreach (var controller in Property.ASPModel.Controllers)
            {
                var controllerstring = renderControllers.GetUsing();
                controllerstring += "\n";
                controllerstring += renderControllers.GetNamespace();
                controllerstring += renderControllers.Open();
                controllerstring += renderControllers.GetControllers(controller.Name);
                controllerstring += renderControllers.Open();
                controllerstring += renderControllers.GetActionResults(controller.actionResults);
                controllerstring += renderControllers.Close();
                controllerstring += renderControllers.Close();
                FileFolder.File(urlcontroller + "/" + controller.Name + "Controller.cs", controllerstring);
            }
        }
    }
}
