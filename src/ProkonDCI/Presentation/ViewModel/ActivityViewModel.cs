using ProkonDCI.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Presentation;

namespace ProkonDCI.Presentation.ViewModel 
{
    public class ActivityViewModel : ObservableObject
    {
        public ActivityViewModel(ActivityDependencyGraph graph, Activity activity)
        {
            Graph = graph;
            Model = activity;
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

        public Point Position
        {
            get
            {   
                return Graph.ActivityPositionFor(Model.Name);
            }
            set
            {
                Graph.ActivityPositionFor(Model, value);
            }
        }
    }
}
