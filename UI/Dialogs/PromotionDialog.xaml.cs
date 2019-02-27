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

namespace Cheops.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for PromotionDialog.xaml
    /// </summary>
    public partial class PromotionDialog : Window
    {
        /// <summary>
        /// Holds a choosen piece type. Is NULL when no piece was choosen.
        /// </summary>
        public PieceType? ChoosenPiece { get; private set; } = null;

        /// <summary>
        /// Holds a sign of choosen piece.
        /// </summary>
        public char ChoosenSign { get; private set; } = '\0';

        char[] signSet;

        /// <summary>
        /// Default constructor for <see cref="PromotionDialog"/>.
        /// </summary>
        /// <param name="side">Side which is to make a promotion.</param>
        /// <param name="imagesSet">Set of images representing the pieces.</param>
        public PromotionDialog(Side side, PieceSet imagesSet)
        {
            InitializeComponent();

            if(side == Side.White)
            {
                xe_Knight_Image.Source = imagesSet.whiteKnight;
                xe_Bishop_Image.Source = imagesSet.whiteBishop;
                xe_Rook_Image.Source = imagesSet.whiteRook;
                xe_Queen_Image.Source = imagesSet.whiteQueen;

                signSet = new char[] {'N','B','R','Q' };
            }
            else
            {
                xe_Knight_Image.Source = imagesSet.blackKnight;
                xe_Bishop_Image.Source = imagesSet.blackBishop;
                xe_Rook_Image.Source = imagesSet.blackRook;
                xe_Queen_Image.Source = imagesSet.blackQueen;

                signSet = new char[] { 'n', 'b', 'r', 'q' };
            }
        }


        void DeselectAll()
        {
            xe_Knight_Border.Background = (Brush)FindResource("NoSelectionGradient");
            xe_Bishop_Border.Background = (Brush)FindResource("NoSelectionGradient");
            xe_Rook_Border.Background = (Brush)FindResource("NoSelectionGradient");
            xe_Queen_Border.Background = (Brush)FindResource("NoSelectionGradient");
        }


        private void em_OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ChoosenSign == '\0')
                e.Cancel = true;
        }

        private void em_OnClick_KnightImage(object sender, MouseButtonEventArgs e)
        {
            DeselectAll();
            ChoosenSign = signSet[0];
            xe_Knight_Border.Background = (Brush)FindResource("SelectionGradient");
            xe_Button_OK.IsEnabled = true;

        }

        private void em_OnClick_BishopImage(object sender, MouseButtonEventArgs e)
        {
            DeselectAll();
            ChoosenSign = signSet[1];
            xe_Bishop_Border.Background = (Brush)FindResource("SelectionGradient");
            xe_Button_OK.IsEnabled = true;
        }

        private void em_OnClick_RookImage(object sender, MouseButtonEventArgs e)
        {
            DeselectAll();
            ChoosenSign = signSet[2];
            xe_Rook_Border.Background = (Brush)FindResource("SelectionGradient");
            xe_Button_OK.IsEnabled = true;
        }

        private void em_OnClick_QueenImage(object sender, MouseButtonEventArgs e)
        {
            DeselectAll();
            ChoosenSign = signSet[3];
            xe_Queen_Border.Background = (Brush)FindResource("SelectionGradient");
            xe_Button_OK.IsEnabled = true;
        }

        private void em_Button_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
