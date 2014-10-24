using Ivento.Dci;
using ProkonDCI.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Operation
{
    public sealed class ResourceAllocation 
    {
        #region Usecase

        // Use case: Allocating Resources
        // Steps: User vs System
        // 1. User wants to have resource planned for activities (trigger)
        //      2. ResourceAllocator resets Resource
        //      3. For each Acitivty in the list
        //      4. ResouceManager allocates resource for Activity

        #endregion

        #region Roles and RoleInterfaces

        internal ResourceAllocation ResourceAllocator { get; private set; }

        internal List<Activity> AllActivities { get { return Model.AllActivities;  } }

        internal ResourceManagerRole ResourceManager { get; private set;  }
        public interface ResourceManagerRole
        {
            void Reset();
            List<Activity> AllocationAt(int weekNo);
        }

        internal Activity Activity { get { return AllActivities.FirstOrDefault(); } }

        private ActivityDependencyGraph Model { get; set; }

        #endregion

        #region Constructors and Role bindings

        public ResourceAllocation(ActivityDependencyGraph model)
        {
            Model = model;
            ResourceManager = Model.Resource;
            ResourceAllocator = this;
        }

        #endregion

        #region Interactions

        /// <summary>
        /// This method executes the Context/use case.
        /// </summary>
        public void Execute()
        {
            Context.Execute(ResourceAllocator.AllocateResources, this);
        }

        #endregion
    }


    #region RoleMethods

    static class ResourceAllocation_ResourceAllocatorRoleMethods
    {
        public static void AllocateResources(this ResourceAllocation resourceAllocator)
        {
            var c = Context.Current<ResourceAllocation>();

            c.ResourceManager.Reset();
            c.AllActivities.ForEach(a => c.ResourceManager.Allocate(a));

        }
    }

    static class ResourceAllocation_ResourceManagerRoleMethods
    {
        public static void Allocate(this ResourceAllocation.ResourceManagerRole resourceManager, Activity activity)
        {
            var c = Context.Current<ResourceAllocation>();

            int tentativeStart = activity.LateStart;

            if (tentativeStart > 0)
            {
                for (int i=0; i<= activity.Duration-1; ++i)
                {
                    var weekAllocations = c.ResourceManager.AllocationAt(tentativeStart+i);
                    for (int j=0; j < activity.ResourceRequirement; ++j)
                    {
                        weekAllocations.Add(activity);
                    }
                }
                activity.PlannedStart = tentativeStart;
            }
        }
    }

    #endregion

}
