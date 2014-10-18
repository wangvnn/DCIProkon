using WPF.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ProkonDCI.SystemOperation;
using ProkonDCI.Domain.Data;
using System.Collections.ObjectModel;
using System.Windows;
using ProkonDCI.Presentation.View;
using System.Windows.Data;

namespace ProkonDCI.Presentation.ViewModel
{
    public class ActivityDependancyViewModel : ObservableObject,
        AddActivityOperation.ActivityViewerRole
    {
        public ActivityDependancyViewModel(ActivityDependencyGraph model)
        {
            Model = model;
            _ActivityDependancyGraph = new CompositeCollection();
            var container1 = new CollectionContainer();
            container1.Collection = Activities;
            var container2 = new CollectionContainer();
            container2.Collection = Dependencies;
            _ActivityDependancyGraph.Add(container1);
            _ActivityDependancyGraph.Add(container2);
        }

        #region Child ViewModel

        public CompositeCollection ActivityDependancyGraph
        {
            get { return _ActivityDependancyGraph;  }
        }
        private CompositeCollection _ActivityDependancyGraph;

        public ObservableCollection<ActivityViewModel> _activities = new ObservableCollection<ActivityViewModel>();
        public ObservableCollection<ActivityViewModel> Activities
        {
            get
            {
                return _activities;
            }
        }

        public ObservableCollection<DependancyViewModel> _dependencies = new ObservableCollection<DependancyViewModel>();
        public ObservableCollection<DependancyViewModel> Dependencies
        { 
            get
            {
                return _dependencies;
            }
        }

        private ActivityViewModel _SelectedItem;
        public ActivityViewModel SelectedItem 
        { 
            get
            {
                return _SelectedItem;
            }
            set 
            {
                _SelectedItem = value;
            }
        }

        public void AddActivity(Activity activity)
        {
            var activityVM = new ActivityViewModel(Model, activity);
            Activities.Add(activityVM);
        }

        #endregion

        #region Model

        private ActivityDependencyGraph Model { get;  set; }

        #endregion

        #region Add Activity

        public ICommand AddActivityCommand
        {
            get
            {
                return new DelegateCommand<Point>(AddActivity);
            }
        }

        private void AddActivity(Point pos)
        {
            new AddActivityOperation(Model, this, new ActivityInfoDialog(), pos).Execute();

            if (Activities.Count >= 2)
            {
                Dependencies.Add(new DependancyViewModel(Activities[0], Activities[1]));
            }
        }
        #endregion
    }
}