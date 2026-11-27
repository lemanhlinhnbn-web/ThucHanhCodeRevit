
using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace ThucHanhCodeRevit
{
    [Transaction(TransactionMode.Manual)]
    internal class _2711_1 : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            // Lấy Document hiện tại
            Document doc =
                commandData.Application.ActiveUIDocument.Document;

            // Lọc tất cả Materials
            IList<Element> matList =
                new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Materials)
                .WhereElementIsNotElementType()
                .ToElements();

            // Tạo chuỗi kết quả
            StringBuilder result = new StringBuilder();

            foreach (Element ele in matList)
            {
                result.AppendLine(
                    ele.Category.Name + "\t" +
                    ele.Name + "\t" +
                    ele.Id);
            }

            // Hiển thị kết quả
            TaskDialog.Show(
                "Danh sách Materials",
                result.ToString());

            return Result.Succeeded;
        }
    }
}
