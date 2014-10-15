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
    // Use case: User adds a new activity
    // Steps: User vs System
    // 1. User wants to add new Activity
    //    2. System asks User to provide Activity's Info
    // 3. User provides Activity Info
    //    4. System records Activity Info
    //    5. System displays Activity on Screen
    public sealed class AddActivityOperation
    {
        #region Roles and RoleInterfaces

        // Roles are an identifier within the Operation/Context.
        // The first role of the AddActivityOperation is ActivityInfoForm.
        // It's important to understand that a Role only exists 
        // inside its Context, and should not be confused with its type.
        // Roles should be private to the Context but cannot in C#, because 
        // they are used in extension methods.
        //                        vvvvvvvvvvvvv
        internal ActivityInfoFormRole ActivityInfoForm  { get; private set; }

        // A Role has a RoleInterface, which is an interface
        // that the object playing this Role must fulfill.
        // In this Operation/Context, the ActivityInfoForm Role must be able to Show 
        // itself to ask the User to provide info
        public interface ActivityInfoFormRole
        {
            void Show();
            event CancelEventHandler BeforeClose;
            Activity GetActivity();
        }

        // Also note the naming convention for roles: If the Role name is "NAME",
        // the RoleInterface type is "NAMERole".

        // The second Role of the AddActivity Operation/Context is NewActivity.
        // Same as for the SourceAccount Role, if you reason about Roles you
        // speak only about their names, not their types. This concept is more 
        // obvious in the Transfer method of the source account below.
        //               vvvvvvvvvvvvvvvvvv
        internal ActivityRepositoryRole ActivityRepository { get; private set; }

        // RoleInterface of the NewActivity Role.
        // In this Context, the NewActivity Role must be able to add 
        // new activity  to itself.
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

        // When the Context is created, the Roles will be bound using the supplied arguments.
        // There can be multiple constructors with different ways of retreiving the objects
        // needed for the binding (database lookup, web service, etc), but there can only be 
        // one BindRoles method, which does the final Role binding.

        // The constructor(s) should be strongly typed to check for errors.

        public AddActivityOperation(ActivityRepositoryRole repository, ActivityViewerRole viewer, Point atPos)
        {
            // The BindRoles method however, should use object so anything can be sent here
            // from the constructors, then casted to the RoleInterface.

            // Make the RolePlayers act the Roles they are supposed to.
            ActivityRepository = repository;
            ActivityInfoForm = (ActivityInfoFormRole)(new ActivityInfoDialog());
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
    
    // RoleMethods are the actual use case implementation, a translation from use case steps
    // to an algorithm. For this MoneyTransfer case, the use case is very simple and will only
    // have one RoleMethod, "Ask".
    static class ActivityInfoFormRoleMethos
    {
        public static void AskForInfo(this AddActivityOperation.ActivityInfoFormRole activityInfoForm)
        {
            // First get a reference to the context/operation, AddActivity.
            // The two parameters are a sanity check that the extension parameter for 
            // the Role "ActivityInfoForm" is actually the same as the Context property.

            // Using this overload of Context.Current, you can be sure that "ActivityInfoForm"
            // and "context.ActivityInfoForm" is the same object. Unless you make a big mistake 
            // in the Context class that is nearly always the case though, so it can 
            // be called without parameters for a simpler syntax.
            var c = Context.Current<AddActivityOperation>(activityInfoForm, ctx => ctx.ActivityInfoForm);

            // Now when we have the context available, it's time for action.
            // The Ask is very simplified here, it just popup a dialog for user to provide info.

            // Note the simplicity and readability of the code. It shows exactly
            // what should happen, and will look very similar to the Use Case description. 

            // Reasoning about this part of the code is easy and gives a nice view how the system works.

            c.ActivityInfoForm.Show();
            c.ActivityInfoForm.BeforeClose += (sender, e) =>
            {
                var d = sender as AddActivityOperation.ActivityInfoFormRole;
    
                try
                {
                    var newActivity = d.GetActivity();
                    if (newActivity != null)
                    {
                        c.ActivityRepository.AddActivity(newActivity);
                        c.ActivityRepository.ActivityPositionFor(newActivity, c.AtPos);
                        c.ActivityViewer.AddActivity(newActivity);
                    } 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bad data info to create activity.");
                    Console.WriteLine("Error::" + ex.Message);
                }
               
            };
        }
    }

    #endregion
}
