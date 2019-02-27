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
    /// Interaction logic for FenDialog.xaml
    /// </summary>
    public partial class FenDialog : Window
    {
        string _outputmsg;

        public string FENstring { get; private set; }

        public FenDialog()
        {
            InitializeComponent();
        }

        private void em_OnTextChange(object sender, TextChangedEventArgs e)
        {
            if (xe_TickImage == null)
                return;

            if(Helper.IsFenValid(xe_FENTextBox.Text, out _outputmsg))
            {
                xe_TickImage.Source = new BitmapImage(new Uri("../../img/Commands/tick.png", UriKind.Relative));
                xe_FENTextBox.Background = Brushes.LawnGreen;
                xe_OutputMessageTextBlock.Text = _outputmsg;
                xe_ButtonOK.IsEnabled = true;
            }
            else
            {
                xe_TickImage.Source = null;
                xe_FENTextBox.Background = Brushes.IndianRed;
                xe_OutputMessageTextBlock.Text = _outputmsg;
                xe_ButtonOK.IsEnabled = false;
            }
        }

        private void em_ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            FENstring = xe_FENTextBox.Text;
            DialogResult = true;
            //Close();
        }

        private void em_FENTextBox_OnFocus(object sender, RoutedEventArgs e)
        {
            xe_FENTextBox.Text = "";
        }
    }
}
