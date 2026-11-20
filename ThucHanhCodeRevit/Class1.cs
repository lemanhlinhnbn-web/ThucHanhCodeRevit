
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace ThucHanhCodeRevit
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            TaskDialog.Show(
                "Kết nối Revit",
                "Xin chào! Tôi đã kết nối thành công với Revit 2024."
            );

            return Result.Succeeded;
        }
    }
}
