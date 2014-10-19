using ProkonDCI.Domain.Data;
using ProkonDCI.Presentation.View;
using ProkonDCI.SystemOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Presentation;

namespace ProkonDCI.Presentation.ViewModel 
{
    public class ActivityViewModel : ObservableObject
    {
        public ActivityViewModel(ObservableObject parent, ActivityDependencyGraph graph, Activity activity)
        {
            Graph = graph;
            Model = activity;
            Parent = parent;
        }

        public ObservableObject Parent { get; private set; }

        public ICommand AddDependancyCommand
        {
            get
            {
                return new DelegateCommand(AddDependancy);
            }
        }

        private void AddDependancy()
        {
            new DependancyAdding(this.Model, new DependantInfoDialog(), Graph, Graph, (DependancyAdding.DependancyViewerRole)Parent).Execute();
        }

        private ActivityDependencyGraph Graph { get; set; }
        private Activity _Model = null;
        private Activity Model 
        { 
            get
            {
                return _Model;
            }
            set
            {
                if (_Model != value)
                {
                    _Model = value;
                    Name = _Model.Name;                    
                }
            }
        }

        private string _activityName = "";
        public string Name 
        {
            get
            {
                return _activityName;
            }
            set
            {
                if (_activityName != value)
                {
                    _activityName = value;
                    RaisePropertyChangedEvent("Name");
                }
            }
        }

        public double PositionX
        {
            get
            {   
                return Graph.ActivityPositionFor(Model.Name).X;
            }
            set
            {
                var p = Graph.ActivityPositionFor(Model.Name);
                Graph.ActivityPositionFor(Model, new Point(value, p.Y));
                RaisePropertyChangedEvent("PositionX");
            }
        }

        public double PositionY
        {
            get
            {
                return Graph.ActivityPositionFor(Model.Name).Y;
            }
            set
            {
                var p = Graph.ActivityPositionFor(Model.Name);
                Graph.ActivityPositionFor(Model, new Point(p.X, value));
                RaisePropertyChangedEvent("PositionY");
            }
        }

        public bool IsSelectable { 
            get { return true; }
        }
    }
}
