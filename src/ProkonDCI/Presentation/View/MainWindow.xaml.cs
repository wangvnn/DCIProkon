using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProkonDCI.Presentation.ViewModel;
using Ivento.Dci;

namespace ProkonDCI.Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
            this.DataContext = new ActivityDependancyViewModel();

            // For this library, the Context must be initialized before use, depending on the type 
            // of application. In a simple application like this, the InStaticScope initalization 
            // can be used. It means that the context will be shared between threads.
            //
            // Multithreading can create unpredictable effects when switching Context, so if the 
            // Context should be scoped per Thread, use InThreadScope.
            // 
            // In a web application, the scope will be per Request and another Initalization
            // method should be called, using for example HttpContext.Items for scope.
            Context.Initialize.InStaticScope();
        }
    }
}
