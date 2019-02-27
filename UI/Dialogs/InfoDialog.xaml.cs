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

namespace Cheops.Dialogs
{
    /// <summary>
    /// Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : Window
    {


        public Color TitleBarColor
        {
            get { return (Color)GetValue(TitleBarColorProperty); }
            set { SetValue(TitleBarColorProperty, value); }
        }

        public static readonly DependencyProperty TitleBarColorProperty =
            DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(InfoDialog), new PropertyMetadata(new Color()));



        public string MessageText
        {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register(nameof(MessageText), typeof(string), typeof(InfoDialog), new PropertyMetadata(string.Empty));



        public string MainButtonText
        {
            get { return (string)GetValue(MainButtonTextProperty); }
            set { SetValue(MainButtonTextProperty, value); }
        }
        
        public static readonly DependencyProperty MainButtonTextProperty =
            DependencyProperty.Register(nameof(MainButtonText), typeof(string), typeof(InfoDialog), new PropertyMetadata(string.Empty));



        public string SideButtonText
        {
            get { return (string)GetValue(SideButtonTextProperty); }
            set { SetValue(SideButtonTextProperty, value); }
        }

        public static readonly DependencyProperty SideButtonTextProperty =
            DependencyProperty.Register(nameof(SideButtonText), typeof(string), typeof(InfoDialog), new PropertyMetadata(string.Empty));


        public InfoDialog()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void GridExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
