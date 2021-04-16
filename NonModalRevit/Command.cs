#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NonModalRevit.View;
using NonModalRevit.Model;
using NonModalRevit.ViewModel;
using System.Collections.ObjectModel;

#endregion

namespace NonModalRevit
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {


            
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Access current selection

            Selection sel = uidoc.Selection;

            // Retrieve elements from database

            FilteredElementCollector col
              = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Wall));

            // Filtered element collector is iterable

            foreach (Element e in col)
            {
                Debug.Print(e.Name);
            }

            // Modify document within a transaction

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Transaction Name");
                tx.Commit();
            }

            var updateEventHandler = new UpdateEventHandler();
            var externalEvent = ExternalEvent.Create(updateEventHandler);

            var mainWindow = new MainWindow();
            

            (mainWindow.DataContext as MainViewModel).MExternalEvent = externalEvent;
            (mainWindow.DataContext as MainViewModel).MExternalEventHandler = updateEventHandler;
            (mainWindow.DataContext as MainViewModel).SelectionCount = sel.GetElementIds().Count;
            (mainWindow.DataContext as MainViewModel).SelectedElementIds = new ObservableCollection<ElementId>(sel.GetElementIds());

            mainWindow.Show();


            return Result.Succeeded;
        }

    }
}
