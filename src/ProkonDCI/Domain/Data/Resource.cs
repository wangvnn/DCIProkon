using ProkonDCI.Domain.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Data
{
    public class Resource :
        ResourceAllocation.ResourceManagerRole
    {
        public Resource()
        {
            Allocations = new Dictionary<int, List<Activity>>();
        }

        public List<Activity> AllocationAt(int weekNo)
        {
            if (!Allocations.ContainsKey(weekNo))
            {
                Allocations.Add(weekNo, new List<Activity>());
            }
            return Allocations[weekNo];
        }

        private Dictionary<int, List<Activity>> _allocations;

        public Dictionary<int, List<Activity>> Allocations
        {
            get { return _allocations; }
            private set { _allocations = value; }
        }

        public void Reset()
        {
            Allocations = new Dictionary<int, List<Activity>>();
        }
    }
}
