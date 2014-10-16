using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProkonDCI.Presentation.ViewModel;
using ProkonDCI.SystemOperation;
using ProkonDCI.Domain.Data;
using Moq;
using System.Windows;

namespace ProkonDCI.Test
{
    [TestClass]
    public class TestActivityDependancy
    {
        [TestMethod]
        public void TestAddActivity()
        {
            //Given an ActivityDependancyView
            var newActivity = new Activity("testActivity", 1, 1);
            var model = new ActivityDependencyGraph();
            var viewer = new ActivityDependancyViewModel(model);
            var atPos = new Point(1,1);
            Mock<AddActivityOperation.ActivityInfoFormRole> infoForm = new Mock<AddActivityOperation.ActivityInfoFormRole>();
            infoForm.Setup( x => x.CreateActivityFromUserInput()).Returns(newActivity);
            
            new AddActivityOperation(model, viewer, infoForm.Object, atPos).Execute();

            Assert.AreEqual(model.ActivityNamed(newActivity.Name), newActivity);
            Assert.AreEqual(viewer.Activities.Count, 1);
        }
    }
}
