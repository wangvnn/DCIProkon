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
    public sealed class DependancyAdding
    {
        #region Usecase
        // Use case: User adds a new dependancy
        // Steps: User vs System
        // 1. User wants to add Dependant of an Activity (trigger)
        //    2. System/DependantInfoFormRole asks User to provide the name of the other Activity
        // 3. User provides the name of the other Activity
        //    4. System/DependancyFactoryRole creates new Dependancy 
        //    5. System/DependancyRepositoryRole stores new Dependancy
        //    6. System/DependancyViewerRole displays Dependancy on Screen
        #endregion

        #region Roles and RoleInterfaces

        internal Activity TheActivity { get; private set; }

        internal DependantInfoFormRole DependantInfoForm { get; private set; }
        public interface DependantInfoFormRole
        {
            bool AskDependantName(out string dependantName);
        }

        internal DependancyFactoryRole DependancyFactory { get; private set; }
        public interface DependancyFactoryRole
        {
            Dependancy CreateDependancy(Activity activity1, Activity activity2);
        }


        internal DependancyRepositoryRole DependancyRepository { get; private set; }
        public interface DependancyRepositoryRole
        {
            void AddDependancy(Dependancy dependancy);
            Activity ActivityNamed(string name);
        }

        internal DependancyViewerRole DependancyViewer { get; private set; }

        public interface DependancyViewerRole
        {
            void AddDependancy(Dependancy dependancy);
        }

        #endregion

        #region Constructors and Role bindings

        public DependancyAdding(Activity theActivity,
                                DependantInfoFormRole infoForm,
                                DependancyFactoryRole factory,
                                DependancyRepositoryRole repository,
                                DependancyViewerRole viewer)
        {
            TheActivity = theActivity;
            DependantInfoForm = infoForm;
            DependancyFactory = factory;
            DependancyRepository = repository;
            DependancyViewer = viewer;
        }

        #endregion

        #region Interactions

        /// <summary>
        /// This method executes the Context/use case.
        /// </summary>
        public void Execute()
        {
            Context.Execute(DependantInfoForm.AskUser, this);
        }

        #endregion
    }

  #region RoleMethods

    static class DependancyAdding_DependantInfoFormRoleMethods
    {
        public static void AskUser(this DependancyAdding.DependantInfoFormRole infoForm)
        {
            var c = Context.Current<DependancyAdding>();

            string dependantName;
            if (c.DependantInfoForm.AskDependantName(out dependantName))
            {
                c.DependancyFactory.CreateNewDependancy(c.TheActivity, dependantName);
            }
        }
    }

    static class DependancyAdding_DependancyFactoryRoleMethods
    {
        public static void CreateNewDependancy(this DependancyAdding.DependancyFactoryRole factory, Activity theActivity, string dependantName)
        {
            var c = Context.Current<DependancyAdding>();

            var dependantActivity = c.DependancyRepository.ActivityNamed(dependantName);
            if (dependantActivity != null)
            {
                var dependancy = c.DependancyFactory.CreateDependancy(c.TheActivity, dependantActivity);
                c.DependancyRepository.AddNewDependancy(dependancy);
            }
            else
            {
                MessageBox.Show("Cannot find dependant Activity.");
            }
        }
    }

    static class DependancyAdding_DependancyRepositoryRoleMethods
    {
        public static void AddNewDependancy(this DependancyAdding.DependancyRepositoryRole repositiry, Dependancy dependancy)
        {
            var c = Context.Current<DependancyAdding>();

            c.DependancyRepository.AddDependancy(dependancy);
            c.DependancyViewer.AddNewDependancy(dependancy);
        }
    }

    static class DependancyAdding_DependancyViewerRoleMethods
    {
        public static void AddNewDependancy(this DependancyAdding.DependancyViewerRole viewer, Dependancy dependancy)
        {
            var c = Context.Current<DependancyAdding>();

            c.DependancyViewer.AddDependancy(dependancy);
        }
    }

    #endregion
}
