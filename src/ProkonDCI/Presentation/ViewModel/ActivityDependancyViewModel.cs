﻿using WPF.Presentation;
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
        ActivityAdding.ActivityViewerRole,
        DependancyAdding.DependancyViewerRole
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

        #endregion

        #region Model

        private ActivityDependencyGraph Model { get;  set; }

        #endregion

        #region Commands

        public ICommand AddActivityCommand
        {
            get
            {
                return new DelegateCommand<Point>(AddActivity);
            }
        }

        private void AddActivity(Point pos)
        {
            new ActivityAdding(new ActivityInfoDialog(), pos, Model, Model, this).Execute();
        }

        #endregion

        #region Public Functions

        public void AddActivity(Activity activity)
        {
            var activityVM = new ActivityViewModel(this, Model, activity);
            Activities.Add(activityVM);
        }

        public void AddDependancy(Dependancy dependancy)
        {
            var source = Activities.FirstOrDefault(a => a.Name == dependancy.FromActivity.Name);
            var target = Activities.FirstOrDefault(a => a.Name == dependancy.ToActivity.Name); 
            var dependancyVM = new DependancyViewModel(source, target);
            Dependencies.Add(dependancyVM);
        }
        #endregion
    }
}