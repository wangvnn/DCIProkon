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
    public class ResourceViewModel : ViewModelBase
    {
        public ResourceViewModel(string name, int resourceIndex, int week)
        {
            Name = name;
            ResourceIndex = resourceIndex;
            Week = week;
        }

        public string Name { get; private set; }
        public int ResourceIndex { get; private set; }
        public int Week { get; private set; }

        public void Refresh()
        {
            RaisePropertyChangedEvent("Name");
            RaisePropertyChangedEvent("ResourceIndex");
            RaisePropertyChangedEvent("Week");
        }
    }
}
