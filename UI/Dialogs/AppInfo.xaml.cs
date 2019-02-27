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
using System.Windows.Shapes;

namespace Cheops.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AppInfo.xaml
    /// </summary>
    public partial class AppInfo : Window
    {
        public AppInfo()
        {
            InitializeComponent();
        }

        private void em_ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
