using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Presentation;

namespace ProkonDCI.Presentation.ViewModel 
{
    public class DependancyViewModel : ObservableObject
    {
        public DependancyViewModel(ActivityViewModel source, ActivityViewModel target)
        {
            Source = source;
            Target = target;

            Source.PropertyChanged += new PropertyChangedEventHandler(
                    SourceOrTargetChanged);

            Target.PropertyChanged += new PropertyChangedEventHandler(
                    SourceOrTargetChanged);
        }

        private void SourceOrTargetChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChangedEvent("PositionX");
            RaisePropertyChangedEvent("PositionY");
            RaisePropertyChangedEvent("X1");
            RaisePropertyChangedEvent("X2");
            RaisePropertyChangedEvent("Y1");
            RaisePropertyChangedEvent("Y2");
        }

        public ActivityViewModel Source { get; private set; }
        public ActivityViewModel Target { get; private set; }

        public double PositionX 
        {
            get
            {
                return Source.PositionX;
            }
            set
            {
            }
        }


        public double PositionY
        {
            get
            {
                return Source.PositionY;
            }
            set
            {
            }
        }
        public double X1
        {
            get
            {
                return 0;
            }
        }


        public double Y1
        {
            get
            {
                return 0;
            }
        }
        public double X2
        {
            get
            {
                return Target.PositionX-Source.PositionX;
            }
        }


        public double Y2
        {
            get
            {
                return Target.PositionY-Source.PositionY;
            }
        }

        public bool IsSelectable
        {
            get { return false; }
        }

        public ICommand RemoveDependancyCommand
        {
            get
            {
                return new DelegateCommand(RemoveDependancy);
            }
        }

        private void RemoveDependancy()
        {
        }
    }
}