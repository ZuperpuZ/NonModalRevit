using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NonModalRevit.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NonModalRevit.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private ObservableCollection<ElementId> _selectedElementIds;
        public ObservableCollection<ElementId> SelectedElementIds
        {
            get => _selectedElementIds;
            set => SetProperty(ref _selectedElementIds, value);
        }


        public ExternalEvent MExternalEvent { get; set; }
        public UpdateEventHandler MExternalEventHandler { get; set; }

        private int _selectionCount;
        public int SelectionCount
        {
            get => _selectionCount;
            set => SetProperty(ref _selectionCount, value);
        }

        private bool _selectionEnCour;

        public bool SelectionEnCour
        {
            get { return _selectionEnCour; }
            set 
            { 
                _selectionEnCour = value;
                _selectionDansRevit.InvokeCanExecuteChanged();
            }
        }


        public MainViewModel()
        {
            _selectionDansRevit = new DelegateCommand(OnClickSelection, CanOnClickSelection);
            SelectionCount = 0;
            _selectionEnCour = true;



        }

        private readonly DelegateCommand _selectionDansRevit;
        public ICommand SelectionDansRevit => _selectionDansRevit;

        private void OnClickSelection(object commandParameter)
        {
            SelectionEnCour = false;

            MExternalEventHandler.Sender = this;

            MExternalEvent.Raise();
            // recup Selection
            
        }

        private bool CanOnClickSelection(object commandParameter)
        {
            return SelectionEnCour;
        }

    }
}
