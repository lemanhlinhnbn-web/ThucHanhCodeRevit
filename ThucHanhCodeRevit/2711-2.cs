
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;

namespace ThucHanhCodeRevit
{
    [Autodesk.Revit.Attributes.Transaction(
        Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _2711_2 : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIDocument uidoc =
                commandData.Application.ActiveUIDocument;

            Document doc = uidoc.Document;

            FilteredElementCollector collector =
                new FilteredElementCollector(doc);

            ElementCategoryFilter filter =
                new ElementCategoryFilter(
                    BuiltInCategory.OST_StructuralFraming);

            IList<Element> wallList = collector
                .WherePasses(filter)
                .WhereElementIsNotElementType()
                .ToElements();

            StringBuilder sb = new StringBuilder();

            foreach (Element wall in wallList)
            {
                sb.AppendLine(wall.Name + "::" + wall.Id.Value);

                foreach (Parameter para in wall.Parameters)
                {
                    sb.Append(getParameterInfomation(
                        para, doc, ":"));

                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            TaskDialog.Show("Thong tin Structural Framing",
                sb.ToString());

            return Result.Succeeded;
        }

        private string getParameterInfomation(
            Parameter para,
            Document doc,
            string seperator)
        {
            string defName = para.Definition.Name;

            switch (para.StorageType)
            {
                case StorageType.Double:
                    return defName + seperator +
                           para.AsValueString();

                case StorageType.ElementId:
                    ElementId id = para.AsElementId();

                    if (id == ElementId.InvalidElementId)
                        return defName + seperator + "-1";

                    Element element = doc.GetElement(id);

                    return defName + seperator +
                        (element != null
                        ? element.Name
                        : id.Value.ToString());

                case StorageType.Integer:
                    return defName + seperator +
                           para.AsInteger().ToString();

                case StorageType.String:
                    return defName + seperator +
                           (para.AsString() ?? "");

                default:
                    return defName + seperator +
                           "Unknown parameter";
            }
        }
    }
}
