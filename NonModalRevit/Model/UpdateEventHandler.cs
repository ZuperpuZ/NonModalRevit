using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using NonModalRevit.ViewModel;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonModalRevit.Model
{
    public class UpdateEventHandler : IExternalEventHandler
    {
        private object sender;

        public object Sender
        {
            get { return sender; }
            set { sender = value; }
        }


        private string propertyName;

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                // Access current selection

                Selection sel = uidoc.Selection;
                TaskDialog.Show("test", "test");

                var references = sel.PickObjects(ObjectType.Element);

                var selection = from reference in references
                                select reference.ElementId;

                (sender as MainViewModel).SelectionCount = references.Count;
                (sender as MainViewModel).SelectedElementIds = new ObservableCollection<ElementId>(selection);

                // Retrieve elements from database
                (sender as MainViewModel).SelectionEnCour = true;
            }
            catch (Exception e)
            {

                TaskDialog.Show("Erreur",e.Message) ;
            }
            
        }

        public string GetName()
        {
            return "ExternalEvent";
        }
    }
}
