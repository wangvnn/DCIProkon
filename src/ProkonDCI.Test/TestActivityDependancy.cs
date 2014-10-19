using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProkonDCI.Presentation.ViewModel;
using ProkonDCI.SystemOperation;
using ProkonDCI.Domain.Data;
using Moq;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace ProkonDCI.Test
{
    [TestClass]
    public class TestActivityDependancy
    {
        [TestMethod]
        public void TestAddActivity()
        {
            // Given a new activity, model, a viewer, a position
            var repository = new ActivityDependencyGraph();
            var viewer = new ActivityDependancyViewModel(repository);
            var atPos = new Point(1,1);
            string name = "testActivity";
            int duration = 1;
            int resource = 1;

            Mock<ActivityAdding.ActivityInfoFormRole> activityInfoForm = new Mock<ActivityAdding.ActivityInfoFormRole>();

            activityInfoForm.Setup(x => x.AskActivityInfo(out name, out duration, out resource)).Returns(true);
            new ActivityAdding(activityInfoForm.Object, atPos, repository, repository, viewer).Execute();

            // Then new activity should be added to the repository 
            // and the viewer
            Assert.AreEqual(viewer.Activities.Count, 1);
        }

        [TestMethod]
        public void TestAddDependancy()
        {
            //Given User added 2 acitvities in the Viewer/Repository            
             var repository = new ActivityDependencyGraph();
            var viewer = new ActivityDependancyViewModel(repository);
            var atPos = new Point(1, 1);
            Mock<ActivityAdding.ActivityInfoFormRole> activityInfoForm = new Mock<ActivityAdding.ActivityInfoFormRole>();

            string name1 = "testActivity1";
            int duration1 = 1;
            int resource1 = 1;

            activityInfoForm.Setup(x => x.AskActivityInfo(out name1, out duration1, out resource1)).Returns(true);
            new ActivityAdding(activityInfoForm.Object, atPos, repository, repository, viewer).Execute();

            string name2 = "testActivity2";
            int duration2 = 1;
            int resource2= 1;

            activityInfoForm.Setup(x => x.AskActivityInfo(out name2, out duration2, out resource2)).Returns(true);
            new ActivityAdding(activityInfoForm.Object, atPos, repository, repository, viewer).Execute();


            Mock<DependancyAdding.DependantInfoFormRole> dependantNameForm = new Mock<DependancyAdding.DependantInfoFormRole>();
            var dependantName = name2;
            dependantNameForm.Setup(x => x.AskDependantName(out dependantName)).Returns(true);

            // When User is adding a Dependancy between acitvity1, activity2
            var activity1 = repository.ActivityNamed(name1);
            new DependancyAdding(activity1, dependantNameForm.Object, repository, repository, viewer).Execute();

            // Then the dependancy should be added to Repository/Viewer
            var activity2 = repository.ActivityNamed(name2);
            Assert.AreEqual(repository.DependanciesToActivity(activity2).First().FromActivity, activity1);
            Assert.AreEqual(repository.DependanciesToActivity(activity2).First().ToActivity, activity2);

            Assert.IsTrue(viewer.Dependencies.Count == 1);
        }
    }
}
