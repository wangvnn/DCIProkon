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
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;

namespace ProkonDCI.Presentation.ViewModel
{
    public class ActivityDependancyViewModel : ViewModelBase
    {
        public ActivityDependancyViewModel(ActivityDependencyGraph model)
        {
            Model = model;
            SetupChildrenViewModel();
            RegisterRoutedCommandHandlers();           
        }

        #region Project Settings
        public string ProjectStart
        {
            get 
            {
                return Model.ProjectStart.ToString();
            }
            set 
            { 
                int projectStart;
                if (int.TryParse(value, out projectStart))
                {
                    Model.ProjectStart = projectStart;
                }                
            }
        }

        public string ProjectFinish
        {
            get
            {
                return Model.ProjectFinish.ToString();
            }
            set
            {
                int projectFinish;
                if (int.TryParse(value, out projectFinish))
                {
                    Model.ProjectFinish = projectFinish;
                }
            }
        }
        #endregion

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

        public ObservableCollection<ResourceViewModel> _resources = new ObservableCollection<ResourceViewModel>();
        public ObservableCollection<ResourceViewModel> Resources
        {
            get
            {
                return _resources;
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
                RaisePropertyChangedEvent("SelectedItem");
            }
        }

        private void SetupChildrenViewModel()
        {
            _ActivityDependancyGraph = new CompositeCollection();

            var container1 = new CollectionContainer();
            container1.Collection = Activities;

            var container2 = new CollectionContainer();
            container2.Collection = Dependencies;

            _ActivityDependancyGraph.Add(container1);
            _ActivityDependancyGraph.Add(container2);
        }

        #endregion

        #region Model

        private ActivityDependencyGraph Model { get;  set; }

        #endregion

        #region Commands

        public ICommand RunCommand
        {
            get
            {
                return new DelegateCommand(Run);
            }
        }

        public void Run()
        {
            Model.DoPlanning();
            
            foreach (var acvitivyVM in Activities)
            {
                acvitivyVM.Refresh();
            }

            RefreshResourceAllocation();
        }

        private void RefreshResourceAllocation()
        {
            Resources.Clear();

            for (int i=Model.ProjectStart; i <= Model.ProjectFinish; ++i)
            {
                var allocation = Model.Resource.AllocationAt(i);
                for (int j=0; j < allocation.Count; ++j)
                {
                    Resources.Add(new ResourceViewModel(
                                        allocation[j].Name,
                                        j,
                                        i));
                }
            }
        }


        public ICommand AddActivityCommand
        {
            get
            {
                return new DelegateCommand<Point>(AddActivity);
            }
        }

        public void AddActivity(Point pos)
        {
            var viewer = this.ActLike<ActivityAdding.ActivityViewerRole>();
    
            new ActivityAdding(new ActivityInfoDialog(), pos, Model, Model, viewer).Execute();
        }

        private void RegisterRoutedCommandHandlers()
        {
            base.RegisterCommand(
                            ActivityRoutedCommands.AddDependancyCommand,
                            param => { return true; },
                            param => this.AddDependancy(param as ActivityViewModel));

            base.RegisterCommand(
                ActivityRoutedCommands.DeleteActivityCommand,
                param => { return true; },
                param => this.DeleteAcitivy(param as ActivityViewModel));

            base.RegisterCommand(
                DependancyRoutedCommands.RemoveDependancyCommand,
                param => { return true; },
                param => this.RemoveDependancy(param as DependancyViewModel));
        }

        public void RemoveDependancy(DependancyViewModel dependancyVM)
        {
            Model.RemoveDependancy(dependancyVM.Model);
            Dependencies.Remove(dependancyVM);
        }

        public void AddDependancy(ActivityViewModel activityVM)
        {
            var viewer = this.ActLike<DependancyAdding.DependancyViewerRole>();
            new DependancyAdding(activityVM.Model, new DependantInfoDialog(), Model, Model, viewer).Execute();
        }

        public void DeleteAcitivy(ActivityViewModel activityVM)
        {
            Model.RemoveActivity(activityVM.Model);
            Activities.Remove(activityVM);
            Dependencies.Where(d => d.Source == activityVM || d.Target == activityVM).ToList().ForEach(x => Dependencies.Remove(x));
        }

        #endregion

        #region Public Functions

        public void AddActivity(Activity activity)
        {
            var activityVM = new ActivityViewModel(Model, activity);
            Activities.Add(activityVM);
        }

        public void AddDependancy(Dependancy dependancy)
        {
            var source = Activities.FirstOrDefault(a => a.Model.Name == dependancy.FromActivity.Name);
            var target = Activities.FirstOrDefault(a => a.Model.Name == dependancy.ToActivity.Name);
            var dependancyVM = new DependancyViewModel(source, target, dependancy);
            Dependencies.Add(dependancyVM);
        }
        #endregion
    }
}