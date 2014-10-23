
using Ivento.Dci;
using ProkonDCI.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Operation
{
    public sealed class FrontLoad 
    {
        #region Usecase
        // Use case: Front Load Operation to calculate the early start time of all activities
        // Steps: User vs System
        // 1. User wants to see the early start of all Activities (trigger)
        //    2. System/FrontLoader plans front load
        //    3. FrontLoader sets all Activities' early start to unplanned
        //    4. FrontLoader finds the Activity that is unplanned 
        //       but all of its predecessors has beed planned 
        //    5. FrontLoader asks the above Activity to frontload itself
        //    6. The Activity then finds the 'max early finish' from its predecessors
        //      6.a If it cannot find the 'max early finish' then its early start = ProjectStart
        //      6.b If it found the 'max early finish' then its early start = max early finish+1
        //    7. Repeat Step 4 until FrontLoader cannot find any unplanned Activity

        #endregion

        #region Roles and RoleInterfaces

        internal FrontLoad FrontLoader { get; private set; }

        internal bool FindUnPlannedActivity()
        {
            UnPlannedActivity = (UnPlannedActivityRole) AllActivities.FirstOrDefault(a => a.EarlyStart == 0 &&
                !Model.PredecessorsOf(a).Any(p => p.EarlyFinish == 0));

            return UnPlannedActivity != null;
        }

        internal UnPlannedActivityRole UnPlannedActivity { get; private set; }
        public interface UnPlannedActivityRole
        {
            int EarlyStart { get; set; }
        }

        internal List<Activity> Predecessors
        {
            get
            {
                return Model.PredecessorsOf((Activity)UnPlannedActivity);
            }
        }

        internal List<Activity> AllActivities { get { return Model.AllActivities;  } }
        internal int ProjectStart { get { return Model.ProjectStart;  } }

        private ActivityDependencyGraph Model { get; set; }

        #endregion

        #region Constructors and Role bindings

        public FrontLoad(ActivityDependencyGraph model)
        {
            Model = model;
            FrontLoader = this;
        }

        #endregion

        #region Interactions

        /// <summary>
        /// This method executes the Context/use case.
        /// </summary>
        public void Execute()
        {
            Context.Execute(FrontLoader.Plans, this);
        }

        #endregion
    }

    #region RoleMethods

    static class FrontLoad_FrontLoaderRoleMethods
    {
        public static void Plans(this FrontLoad frontLoader)
        {
            var c = Context.Current<FrontLoad>();

            c.AllActivities.ForEach(a => a.EarlyStart = 0);

            while (c.FrontLoader.FindUnPlannedActivity())
            {
                c.UnPlannedActivity.FrontLoad();

            }
        }
    }

    static class FrontLoad_UnPlannedActivityRoleMethods
    {
        public static void FrontLoad(this FrontLoad.UnPlannedActivityRole unplannedActivity)
        {
            var c = Context.Current<FrontLoad>();

            Activity maxPred = c.Predecessors.FirstOrDefault(p => p.EarlyFinish == c.Predecessors.Max(m => m.EarlyFinish));
            if (maxPred != null)
            {
                c.UnPlannedActivity.EarlyStart = maxPred.EarlyFinish + 1;
            }
            else
            {
                c.UnPlannedActivity.EarlyStart = c.ProjectStart;
            }
        }
    }    

    #endregion
}
