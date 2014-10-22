using ProkonDCI.Domain.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Data
{
    public class Activity:
        FrontLoad.UnPlannedActivityRole,
        BackLoad.UnPlannedActivityRole
    {
        public Activity(string name, int duration, int resourceRequirement)
        {
            Name = name;
            Duration = duration;
            ResourceRequirement = resourceRequirement;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
            private set { _duration = value; }
        }

        private int _resourceRequirement;
        public int ResourceRequirement
        {
            get { return _resourceRequirement; }
            private set { _resourceRequirement = value; }
        }

        private int _earlyStart;

        public int EarlyStart
        {
            get { return _earlyStart; }
            set { _earlyStart = value; }
        }
        public int LateStart
        {
            get { return _lateFinish > 0 ? _lateFinish - _duration + 1 : 0; }
        }

        private int _lateFinish;
        public int LateFinish
        {
            get { return _lateFinish; }
            set { _lateFinish = value; }
        }
        public int EarlyFinish
        {
            get { return _earlyStart + _duration - 1; }
        }

        private int _plannedStart;

        public int PlannedStart
        {
            get { return _plannedStart; }
            set { _plannedStart = value; }
        }
        public int PlannedFinish
        {
            get { return _plannedStart > 0 ? _plannedStart + _duration - 1 : 0; }
        }

    }
}
