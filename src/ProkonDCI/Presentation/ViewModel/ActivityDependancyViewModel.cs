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

namespace ProkonDCI.Presentation.ViewModel
{
    public class ActivityDependancyViewModel : ObservableObject,
        AddActivityOperation.ActivityViewerRole
    {
        public ActivityDependancyViewModel(ActivityDependencyGraph model)
        {
            Model = model;
        }

        #region Child ViewModel

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

        public void AddActivity(Activity activity)
        {
            var activityVM = new ActivityViewModel(Model, activity);
            Activities.Add(activityVM);
        }

        #endregion

        #region Model

        private ActivityDependencyGraph Model { get;  set; }

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
            new AddActivityOperation(Model, this, pos).Execute();
        }
        #endregion

        #endregion
    }
}