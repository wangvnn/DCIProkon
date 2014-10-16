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
    public sealed class AddActivityOperation
    {
        #region Roles and RoleInterfaces

        internal ActivityInfoFormRole ActivityInfoForm  { get; private set; }
        public interface ActivityInfoFormRole
        {
            Activity CreateActivityFromUserInput();
        }

        internal ActivityRepositoryRole ActivityRepository { get; private set; }
        public interface ActivityRepositoryRole
        {
            void AddActivity(Activity activity);
            void ActivityPositionFor(Activity activity, Point p);
        }

        internal ActivityViewerRole ActivityViewer { get; private set; }

        public interface ActivityViewerRole
        {
            void AddActivity(Activity activity);
        }

        internal Point AtPos { get; private set; }

        #endregion

        #region Constructors and Role bindings

        public AddActivityOperation(ActivityRepositoryRole repository, 
                                    ActivityViewerRole viewer,
                                    ActivityInfoFormRole infoForm, 
                                    Point atPos)
        {
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
            // Use case: User adds a new activity
            // Steps: User vs System
            // 1. User wants to add new Activity (trigger)
            //    2. System asks User to provide Activity's Info
            // 3. User provides Activity Info
            //    4. System creates new Activity from the info
            //    5. ActivityRepository stores new Activity
            //    6. ActivityViewer displays Activity on Screen
            Context.Execute(ActivityInfoForm.AskForInfo, this);
        }

        #endregion
    }

  #region RoleMethods
    
    static class ActivityInfoFormRoleMethos
    {
        public static void AskForInfo(this AddActivityOperation.ActivityInfoFormRole activityInfoForm)
        {
            var c = Context.Current<AddActivityOperation>(activityInfoForm, ctx => ctx.ActivityInfoForm);

            try
            {
                var newActivity = c.ActivityInfoForm.CreateActivityFromUserInput(); 
                if (newActivity != null)
                {
                    c.ActivityRepository.AddActivity(newActivity);
                    c.ActivityRepository.ActivityPositionFor(newActivity, c.AtPos);
                    c.ActivityViewer.AddActivity(newActivity);
                } 
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Bad data info to create activity.");
                Console.WriteLine("Error::" + ex.Message);
            }
  
        }
    }

    #endregion
}
