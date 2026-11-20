
using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;

namespace ThucHanhCodeRevit
{
    [Transaction(TransactionMode.Manual)]
    public class _2011_1 : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            // Lấy tài liệu Revit đang hoạt động
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            // Chọn các cạnh (Edge) trong Revit
            IList<Reference> referenceCollection =
                uidoc.Selection.PickObjects(ObjectType.Edge);

            // Hiển thị tổng số cạnh đã chọn
            MessageBox.Show(
                "You have selected total "
                + referenceCollection.Count.ToString()
                + " Edges."
            );

            return Result.Succeeded;
        }
    }
}
