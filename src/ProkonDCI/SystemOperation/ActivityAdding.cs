using Ivento.Dci;
using ProkonDCI.Domain.Data;
using ProkonDCI.Presentation.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ProkonDCI.SystemOperation
{
    public sealed class ActivityAdding
    {
        #region Usecase
        // Use case: User adds a new activity
        // Steps: User vs System
        // 1. User wants to add new Activity (trigger)
        //    2. System/ActivityInfoFormRole asks User to provide Activity's Info
        // 3. User provides Activity Info
        //    4. System/ActivityFactoryRole creates new Activity from the info
        //    5. System/ActivityRepositoryRole stores new Activity
        //    6. System/ActivityViewerRole displays Activity on Screen
        // Extension
        // 3.a User provides bad data info: duplicated name, non numberic values...
        //    4.a System/MessageBox reports error 
        #endregion

        #region Roles and RoleInterfaces

        internal ActivityInfoFormRole ActivityInfoForm { get; private set; }
        public interface ActivityInfoFormRole
        {
            bool AskActivityInfo(out string name, out int duration, out int resource);
        }

        internal ActivityFactoryRole ActivityFactory { get; private set; }
        public interface ActivityFactoryRole
        {
            Activity CreateActivity(string name, int duration, int resource);
         }
        
        internal ActivityRepositoryRole ActivityRepository { get; private set; }
        public interface ActivityRepositoryRole
        {
            void AddActivity(Activity activity);
            void ActivityPositionFor(Activity activity, Point p);
            Activity ActivityNamed(string name);
        }

        internal ActivityViewerRole ActivityViewer { get; private set; }

        public interface ActivityViewerRole
        {
            void AddActivity(Activity activity);
        }

        internal Point AtPos { get; private set; }

        #endregion

        #region Constructors and Role bindings

        public ActivityAdding(ActivityInfoFormRole infoForm, 
                              Point atPos,
                              ActivityFactoryRole factory,                             
                              ActivityRepositoryRole repository,
                              ActivityViewerRole viewer)
        {
            ActivityFactory = factory;
            ActivityRepository = repository;
            ActivityInfoForm = infoForm;
            ActivityViewer = viewer;
            AtPos = atPos;
        }

        #endregion

        #region Interactions

        /// <summary>
        /// This method executes the Context/use case.
        /// </summary>
        public void Execute()
        {
            Context.Execute(ActivityInfoForm.AskForInfo, this);
        }

        #endregion
    }

    #region RoleMethods

    static class ActivityAdding_ActivityInfoFormRoleMethos
    {
        public static void AskForInfo(this ActivityAdding.ActivityInfoFormRole activityInfoForm)
        {
            var c = Context.Current<ActivityAdding>();

            try
            {
                string name;
                int duration;
                int resource;
                if (c.ActivityInfoForm.AskActivityInfo(out name, out duration, out resource))
                {
                    c.ActivityFactory.CreateNewActivity(name, duration, resource);
                } 
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Bad data info to create activity.");
                Console.WriteLine("Error::" + ex.Message);
            }  
        }
    }

    static class ActivityAdding_ActivityFactoryRoleMethods
    {
        public static void CreateNewActivity(this ActivityAdding.ActivityFactoryRole factory, string name, int duration, int resource)
        {
            var c = Context.Current<ActivityAdding>();

            if (c.ActivityRepository.ActivityNamed(name) == null)
            {

                var newActivity = c.ActivityFactory.CreateActivity(name, duration, resource);

                c.ActivityRepository.AddNewActivity(newActivity, c.AtPos);
            }
            else
            {
                MessageBox.Show("There is an acitivty with the same name.", "Error");
            }
        }
    }

    static class ActivityAdding_ActivityRepositoryRoleMethods
    {
        public static void AddNewActivity(this ActivityAdding.ActivityRepositoryRole repository, Activity newActivity, Point atPos)
        {
            var c = Context.Current<ActivityAdding>();

            c.ActivityRepository.AddActivity(newActivity);
            c.ActivityRepository.ActivityPositionFor(newActivity, atPos);

            c.ActivityViewer.AddNewActivity(newActivity);
        }
    }

    static class ActivityAdding_ActivityViewRoleMethods
    {
        public static void AddNewActivity(this ActivityAdding.ActivityViewerRole viever, Activity newActivity)
        {
            var c = Context.Current<ActivityAdding>();
            c.ActivityViewer.AddActivity(newActivity);
        }
    }

    #endregion
}