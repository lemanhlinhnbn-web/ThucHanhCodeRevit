
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace ThucHanhCodeRevit
{
    public class App : IExternalApplication
    {
        private const string TabName = "THUC HANH REVIT API";
        private const string PanelName = "Bai Thuc Hanh";

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Tao Ribbon Tab
                try
                {
                    application.CreateRibbonTab(TabName);
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException)
                {
                    // Tab da ton tai
                }

                // Tao Ribbon Panel
                var panels = application.GetRibbonPanels(TabName);

                RibbonPanel panel = panels.FirstOrDefault(
                    p => p.Name == PanelName);

                if (panel == null)
                {
                    panel = application.CreateRibbonPanel(
                        TabName, PanelName);
                }

                // Lay assembly hien tai
                Assembly assembly = Assembly.GetExecutingAssembly();

                string assemblyPath = assembly.Location;

                // Tu dong tim tat ca ExternalCommand
                var commandTypes = assembly.GetTypes()
                    .Where(t =>
                        t.IsClass &&
                        !t.IsAbstract &&
                        typeof(IExternalCommand).IsAssignableFrom(t))
                    .OrderBy(t => t.Name)
                    .ToList();

                // Tao nut lenh cho tung class
                foreach (Type commandType in commandTypes)
                {
                    string className = commandType.FullName;

                    string buttonName = className
                        .Replace(".", "_")
                        .Replace("+", "_");

                    PushButtonData buttonData =
                        new PushButtonData(
                            buttonName,
                            commandType.Name,
                            assemblyPath,
                            className
                        );

                    buttonData.ToolTip =
                        "Chay bai thuc hanh: " + commandType.Name;

                    // Them nut vao Ribbon
                    panel.AddItem(buttonData);
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show(
                    "Revit API",
                    "Loi khoi tao Add-in:\n" + ex.Message);

                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
