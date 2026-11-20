using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ThucHanhCodeRevit
{
    [Transaction(TransactionMode.Manual)]
    public class _2011_2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            ICollection<ElementId> selectedIds = uiDocument.Selection.GetElementIds();
            TaskDialog.Show("Revit", "Số lượng đối tượng được chọn: " + selectedIds.Count);

            ICollection<ElementId> selectedWallIds = new List<ElementId>();
            ICollection<ElementId> selectedFloorIds = new List<ElementId>();
            ICollection<ElementId> selectedColumnIds = new List<ElementId>();

            // Lọc Tường, Sàn trong những đối tượng đang chọn
            foreach (ElementId id in selectedIds)
            {
                Element el = document.GetElement(id);
                if (el is Wall) selectedWallIds.Add(id);
                if (el is Floor) selectedFloorIds.Add(id);
            }

            // Lấy Cột bằng FilteredElementCollector (toàn bộ cột trong model)
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ICollection<Element> elementsColumns = collector
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .ToElements();

            foreach (Element col in elementsColumns)
            {
                selectedColumnIds.Add(col.Id);
            }

            // Tường
            uiDocument.Selection.SetElementIds(selectedWallIds);
            if (selectedWallIds.Count != 0)
                TaskDialog.Show("Revit", selectedWallIds.Count + " đối tượng Tường đã được chọn!");
            else
                TaskDialog.Show("Revit", "Không có đối tượng tường nào được chọn!");

            // Cột
            uiDocument.Selection.SetElementIds(selectedColumnIds);
            if (selectedColumnIds.Count != 0)
                TaskDialog.Show("Revit", selectedColumnIds.Count + " đối tượng Cột đã được chọn!");
            else
                TaskDialog.Show("Revit", "Không có đối tượng Cột nào được chọn!");

            return Result.Succeeded;
        }
    }
}