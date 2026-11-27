
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace ThucHanhCodeRevit
{
    [Transaction(TransactionMode.Manual)]
    public class _2711_3 : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            // Lay Document hien tai
            UIDocument uidoc =
                commandData.Application.ActiveUIDocument;

            Document doc = uidoc.Document;

            // Tao bo loc Structural Column
            FilteredElementCollector collector =
                new FilteredElementCollector(doc);

            ElementCategoryFilter filter =
                new ElementCategoryFilter(
                    BuiltInCategory.OST_StructuralColumns);

            // Lay danh sach cac cot
            IList<Element> columnList = collector
                .WherePasses(filter)
                .WhereElementIsNotElementType()
                .ToElements();

            // Tao StringBuilder de hien thi ket qua
            StringBuilder output = new StringBuilder();

            output.AppendLine(
                "Tat ca Structural Column trong BIM model: "
                + columnList.Count);

            output.AppendLine("--------------------------------");

            // Duyet tung cot
            foreach (Element e1 in columnList)
            {
                output.AppendLine("Element ID: " + e1.Id);

                // Lay ElementType cua cot
                ElementType type =
                    doc.GetElement(e1.GetTypeId()) as ElementType;

                if (type == null)
                {
                    output.AppendLine("Khong tim thay ElementType");
                    output.AppendLine();
                    continue;
                }

                // Lay Parameter h va b
                Parameter h = type.LookupParameter("h");
                Parameter b = type.LookupParameter("b");

                if (h != null && b != null
                    && h.StorageType == StorageType.Double
                    && b.StorageType == StorageType.Double)
                {
                    // Chuyen doi Feet sang Millimeter
                    double hh1 =
                        ChangeUnitFeetToMillimeter(h.AsDouble());

                    double bb1 =
                        ChangeUnitFeetToMillimeter(b.AsDouble());

                    output.AppendLine("h = " + hh1 + " mm");
                    output.AppendLine("b = " + bb1 + " mm");
                }
                else
                {
                    output.AppendLine(
                        "Khong tim thay Parameter h hoac b");
                }

                output.AppendLine("--------------------------------");
            }

            // Hien thi ket qua
            TaskDialog.Show("Ket qua", output.ToString());

            return Result.Succeeded;
        }

        // Ham chuyen doi Feet sang Millimeter
        public static double ChangeUnitFeetToMillimeter(double Value)
        {
            double value = UnitUtils.ConvertFromInternalUnits(
                Value,
                UnitTypeId.Millimeters);

            return Math.Round(value, 2);
        }
    }
}
