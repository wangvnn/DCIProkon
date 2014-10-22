using ProkonDCI.Domain.Data;
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
    public class DependancyRoutedCommands
    {
        public static readonly RoutedUICommand RemoveDependancyCommand = 
            new RoutedUICommand("To remove dependancy", "RemoveDependancy", typeof(DependancyRoutedCommands));
    }

    public class DependancyViewModel : ViewModelBase
    {
        public DependancyViewModel(ActivityViewModel source, ActivityViewModel target, Dependancy dependnacy)
        {
            Model = dependnacy;
            Source = source;
            Target = target;

            Source.PropertyChanged += new PropertyChangedEventHandler(
                    SourceOrTargetChanged);

            Target.PropertyChanged += new PropertyChangedEventHandler(
                    SourceOrTargetChanged);
        }

        public Dependancy Model {get; private set; }

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
                return Source.NodeWidth;
            }
        }


        public double Y1
        {
            get
            {
                return Source.NodeHeight/2;
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
                return Target.PositionY-Source.PositionY + Target.NodeHeight/2;
            }
        }
    }
}