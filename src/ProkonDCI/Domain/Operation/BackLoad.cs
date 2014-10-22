
using Ivento.Dci;
using ProkonDCI.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Operation
{
    public sealed class BackLoad 
    {
        #region Usecase
        // Use case: Back Load Operation to calculate the late finish time of all activities
        // Steps: User vs System
        // 1. User wants to see the late finish of all Activities (trigger)
        //    2. System/BackLoader plans back load
        //    3. BackLoader sets all Activities' late finish to unplanned
        //    4. BackLoader finds the Activity that is unplanned 
        //       and has its predecessors unplanned too
        //    5. BackLoader asks the above Activity to backload itself
        //    6. The Activity then finds the 'min late start' from its Successors
        //      6.a If it cannot find the 'min late start' then its late Finish = ProjectFinish
        //      6.b If it found the 'min late start' then its late Finish = min late start-1
        //    7. Repeat Step 4 until BackLoader cannot find any unplanned Activity

        #endregion

        #region Roles and RoleInterfaces

        internal BackLoad BackLoader { get; private set; }

        internal bool FindUnPlannedActivity()
        {
            UnPlannedActivity = (UnPlannedActivityRole) AllActivities.FirstOrDefault(a => a.LateFinish == 0 &&
                !Model.SuccessorsOf(a).Any(p => p.LateStart == 0));

            return UnPlannedActivity != null;
        }

        internal UnPlannedActivityRole UnPlannedActivity { get; private set; }
        public interface UnPlannedActivityRole
        {
            int LateFinish { get; set; }
        }

        internal List<Activity> Successors { 
            get 
            {
                return Model.SuccessorsOf((Activity)UnPlannedActivity);
            } 
        }
        internal List<Activity> AllActivities { get { return Model.AllActivities;  } }
        internal int ProjectFinish { get { return Model.ProjectFinish;  } }

        private ActivityDependencyGraph Model { get; set; }

        #endregion

        #region Constructors and Role bindings

        public BackLoad(ActivityDependencyGraph model)
        {
            Model = model;
            BackLoader = this;
        }

        #endregion

        #region Interactions

        /// <summary>
        /// This method executes the Context/use case.
        /// </summary>
        public void Execute()
        {
            Context.Execute(BackLoader.Plans, this);
        }

        #endregion
    }

    #region RoleMethods

    static class BackLoad_BackLoaderRoleMethods
    {
        public static void Plans(this BackLoad BackLoader)
        {
            var c = Context.Current<BackLoad>();

            c.AllActivities.ForEach(a => a.LateFinish = 0);

            while (c.BackLoader.FindUnPlannedActivity())
            {
                c.UnPlannedActivity.BackLoad();
            }
        }
    }

    static class BackLoad_UnPlannedActivityRoleMethods
    {
        public static void BackLoad(this BackLoad.UnPlannedActivityRole unplannedActivity)
        {
            var c = Context.Current<BackLoad>();

            Activity maxPred = c.Successors.FirstOrDefault(p => p.LateStart == c.Successors.Min(m => m.LateStart));
            if (maxPred != null)
            {
                c.UnPlannedActivity.LateFinish = maxPred.LateStart - 1;
            }
            else
            {
                c.UnPlannedActivity.LateFinish = c.ProjectFinish;
            }
        }
    }    

    #endregion
}