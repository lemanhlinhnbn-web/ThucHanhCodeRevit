using System;
using System.Reflection;
using Autodesk.Revit.UI;

namespace ThucHanhCodeRevit
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            try
            {
                string dll = Assembly.GetExecutingAssembly().Location;

                try { app.CreateRibbonTab("LTBIM"); } catch { }
                RibbonPanel panel = app.CreateRibbonPanel("LTBIM", "Thực hành");

                var btn = new PushButtonData(
                    "btn2011_1", "Chọn\ncạnh", dll, "ThucHanhCodeRevit._2011_1");
                btn.ToolTip = "Chọn nhiều cạnh và đếm số lượng";
                panel.AddItem(btn);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Lỗi OnStartup", ex.ToString());
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}