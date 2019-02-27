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
using Cheops.ChessBoardComponent;
using System.ComponentModel;

namespace Cheops.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for Analyse.xaml
    /// </summary>
    public partial class Analyse : Window
    {
        int _depth = 0;
        SmartBoard _board;

        public bool IsDepthOK { get; private set; } = true;
        public bool IsAlgorithmOK { get; private set; } = false;
        //public bool IsCheckBoxOK { get; private set; }

        List<string> _algorithms = new List<string>
        {
            "Min-Max",
            "Alfa-Beta",
            "Alfa-Beta + Ruchy zabójcy",
            "Alfa-Beta + Ruchy zabójcy + Sortowanie",
            "Alfa-Beta + Ruchy zabójcy + Sortowanie + Tablice Historii",
            "Alfa-Beta + Ruchy zabójcy + Sortowanie + Pusty Ruch",
            "Alfa-Beta + Ruchy zabójcy + Sortowanie + Pusty Ruch + Tablice Transpozycji",
        };

        public Analyse(SmartBoard board)
        {
            InitializeComponent();

            xe_DDList.ItemsSource = _algorithms;
            xe_Board.PieceSet = new PieceSet("../img/PieceSet/Neo");
            xe_Board.SetPositionFromFEN(board.LogicalBoard.ExportFEN());

            _board = board;
        }

        private void xe_Depth_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(xe_Depth_TextBox.Text, out _depth) && _depth > 0 && _depth < 10)
            {
                IsDepthOK = true;
                xe_Depth_TextBox.Background = Brushes.LightGreen;
            }
            else
            {
                IsDepthOK = false;
                xe_Depth_TextBox.Background = Brushes.PaleVioletRed;
            }

            if (xe_Button_OK != null)
            {
                if (IsDepthOK && IsAlgorithmOK)
                    xe_Button_OK.IsEnabled = true;
                else
                    xe_Button_OK.IsEnabled = false; 
            }  
             
        }

        private void xe_DDList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(xe_DDList.SelectedIndex != -1)
            {
                IsAlgorithmOK = true;
            }
            else
            {
                IsAlgorithmOK = false;
            }


            if (IsDepthOK && IsAlgorithmOK)
                xe_Button_OK.IsEnabled = true;
            else
                xe_Button_OK.IsEnabled = false;

            if (xe_DDList.SelectedIndex < 2)
            {
                xe_checkBox.IsEnabled = false;
                xe_checkBox.IsChecked = false;
            }
            else
                xe_checkBox.IsEnabled = true;
        }

        private void xe_Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Worker workerWindow = new Worker(xe_DDList.SelectedIndex, _depth, _board, (bool)xe_checkBox.IsChecked);
            Hide();
            workerWindow.ShowDialog();
            Close();
        }
    }
}
