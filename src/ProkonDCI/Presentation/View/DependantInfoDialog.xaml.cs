using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using ProkonDCI.Domain.Data;
using ProkonDCI.SystemOperation;
using System.Windows.Threading;

namespace ProkonDCI.Presentation.View
{    
    /// <summary>
    /// Interaction logic for ActivityInfoDialog.xaml
    /// </summary>
    public partial class DependantInfoDialog : Window, 
        DependancyAdding.DependantInfoFormRole
    {
        public DependantInfoDialog()
        {
            InitializeComponent();            
        }

        private bool Canceled { get; set; }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Canceled = true;
            Close();
        }

        private void BtnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Canceled = false;
            Close();
        }

        public bool AskDependantName(out string dependantName)
        {
            dependantName = "";
            this.ShowDialog();

            if (!Canceled)
            {
                dependantName = NameInput.Text;
                return true;
            }            

            return false;
        }
    }
}
