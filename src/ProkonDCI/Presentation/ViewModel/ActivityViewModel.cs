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
    public class ActivityRoutedCommands
    {
        public static readonly RoutedUICommand AddDependancyCommand =
            new RoutedUICommand("To add dependancy", "Add Dependancy", typeof(ActivityRoutedCommands));
        public static readonly RoutedUICommand DeleteActivityCommand =
            new RoutedUICommand("To delete activity", "Delete Activity", typeof(ActivityRoutedCommands));
    }

    public class ActivityViewModel : ViewModelBase
    {
        public ActivityViewModel(ActivityDependencyGraph graph, Activity activity)
        {
            Graph = graph;
            Model = activity;
        }      

        private ActivityDependencyGraph Graph { get; set; }
        private Activity _Model = null;
        public Activity Model 
        { 
            get
            {
                return _Model;
            }
            private set
            {
                if (_Model != value)
                {
                    _Model = value;              
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

        public int NodeWidth
        {
            get { return 60; }
        }

        public int NodeHeight
        {
            get { return 30; }
        }
    }
}
