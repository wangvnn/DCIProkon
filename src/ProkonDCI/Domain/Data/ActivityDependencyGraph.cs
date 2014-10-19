using ProkonDCI.SystemOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace ProkonDCI.Domain.Data
{
    public class ActivityDependencyGraph : 
        ActivityAdding.ActivityRepositoryRole,
        ActivityAdding.ActivityFactoryRole,
        DependancyAdding.DependancyRepositoryRole,
        DependancyAdding.DependancyFactoryRole
    {
        public ActivityDependencyGraph()
        {
        }

        #region Properties
        public Resource Resource
        {
            get { return _resource; }
            private set { _resource = value; }
        }
        private int _projectStart = 0;

        public int ProjectStart
        {
            get { return _projectStart; }
            set { _projectStart = value; }
        }
        private int _projectFinish = 0;

        public int ProjectFinish
        {
            get { return _projectFinish; }
            set { _projectFinish = value; }
        }
        #endregion

        #region Factory Methods
        public Dependancy CreateDependancy(Activity activity1, Activity activity2)
        {
            return new Dependancy(activity1, activity2);
        }

        public Activity CreateActivity(string name, int duration, int resource)
        {
            return new Activity(name, duration, resource);
        }
        #endregion

        #region READ Operations
        public Activity ActivityNamed(string name)
        {
            return _activities.Where(a => a.Name == name).FirstOrDefault();
        }
        public Point ActivityPositionFor(string name)
        {
            if (_activityPositions.ContainsKey(name))
                return _activityPositions[name];
            else
                return new Point(0,0);
        }

        public List<Dependancy> DependanciesFromActivity(Activity activity)
        {
            return _dependancies.Where(d => d.FromActivity.Name == activity.Name).ToList();
        }
        public List<Dependancy> DependanciesToActivity(Activity activity)
        {
            return _dependancies.Where(d => d.ToActivity.Name == activity.Name).ToList();
        }
        public List<Activity> PredecessorsOf(Activity activity)
        {
            return DependanciesToActivity(activity).Select(d => d.FromActivity).ToList();
        }

        public List<Activity> SuccessorsOf(Activity activity)
        {
            return DependanciesFromActivity(activity).Select(d => d.ToActivity).ToList();
        }
        #endregion

        #region WRITE Operations
        public void ActivityPositionFor(Activity activity, Point putPosition)
        {
            _activityPositions[activity.Name] = putPosition;
        }

        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);
        }

        public void AddDependancy(Dependancy dependancy)
        {
            _dependancies.Add(dependancy);
        }

        public void RemoveActivity(Activity activity)
        {
            _activities.Remove(activity);
            if (_activityPositions.ContainsKey(activity.Name))
                _activityPositions.Remove(activity.Name);
            _dependancies.Where(d => d.FromActivity.Name == activity.Name || d.ToActivity.Name == activity.Name).ToList().ForEach(x => _dependancies.Remove(x));
        }

        public void RemoveDependancy(Dependancy dependancy)
        {
            _dependancies.Remove(dependancy);
        }
        
        #endregion

        #region Private Fields
        private List<Activity> _activities = new List<Activity>();
        private Dictionary<string, Point> _activityPositions = new Dictionary<string, Point>();
        private List<Dependancy> _dependancies = new List<Dependancy>();
        private Resource _resource = new Resource();
#       endregion
    }
}
