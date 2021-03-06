﻿using System;
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
    public partial class ActivityInfoDialog : Window, 
        ActivityAdding.ActivityInfoFormRole
    {
        public ActivityInfoDialog()
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

        public bool AskActivityInfo(out string name, out int duration, out int resource)
        {
            name = "";
            duration = 0;
            resource = 0;

            this.ShowDialog();

            if (!Canceled)
            {
                try
                {
                    name = NameInput.Text;
                    duration = int.Parse(DurationInput.Text);
                    resource = int.Parse(ResourceRequirementInput.Text);
                    return true;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Error::" + ex.Message);
                    throw;
                }
            }

            return false;
        }
    }
}
