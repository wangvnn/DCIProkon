using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Data
{
    public class Dependancy
    {
        public Dependancy(Activity from, Activity to)
        {
            FromActivity = from;
            ToActivity = to;
        }

        private Activity _fromActivity;

        public Activity FromActivity
        {
            get { return _fromActivity; }
            private set { _fromActivity = value; }
        }
        private Activity _toActivity;

        public Activity ToActivity
        {
            get { return _toActivity; }
            set { _toActivity = value; }
        }
     
    }
}
