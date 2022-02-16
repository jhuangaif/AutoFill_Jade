using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using AIFAutoFillDB.ViewModel;

namespace AIFAutoFillDB.View
{
    public partial class AIFCIMainWindow : Window
    {
        public AIFCIMainWindow()
        {
            if (Application.Current.MainWindow == null)
            {
                Application.Current.MainWindow = this;
            }

            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
