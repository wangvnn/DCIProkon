using WPF.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ProkonDCI.SystemOperation;
using ProkonDCI.Domain.Data;
using ProkonDCI.Presentation.View;

namespace ProkonDCI.Presentation.ViewModel
{
    public class ActivityDependancyViewModel : ObservableObject
    {
        public ActivityDependancyViewModel()
        {
            Model = new ActivityDependencyGraph();
        }

        private ActivityDependencyGraph Model { get;  set; }

        #region Add Activity
        public ICommand AddActivityCommand
        {
            get
            {
                return new DelegateCommand(AddActivity);
            }
        }

        private void AddActivity()
        {
            new AddActivityOperation(Model, new ActivityInfoDialog()).Execute();
        }
        #endregion
    }
}