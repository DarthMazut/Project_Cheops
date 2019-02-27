using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cheops
{
    class Commands
    {
        public static readonly RoutedUICommand RotateBoard = new RoutedUICommand
       (
           "Obróć plansze",
           "Obróć plansze",
           typeof(Commands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.R,ModifierKeys.Control)
           }
       );

        public static readonly RoutedUICommand NewBoard = new RoutedUICommand
        (
            "Ustaw pozycje startowa",
            "Ustaw pozycje startowa",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N,ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ChangeColor = new RoutedUICommand
        (
            "Zmien kolor",
            "Zmien kolor",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L,ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand UndoMove = new RoutedUICommand
        (
            "Cofnij ruch",
            "Cofnij ruch",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Z,ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand FenImport = new RoutedUICommand
       (
           "Importuj pozycje",
           "Importuj pozycje",
           typeof(Commands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.F,ModifierKeys.Control)
           }
       );

        public static readonly RoutedUICommand EvaluatePosition = new RoutedUICommand
       (
           "Ocen pozycje",
           "Ocen pozycje",
           typeof(Commands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.E,ModifierKeys.Control)
           }
       );

        public static readonly RoutedUICommand PerformAnalyse = new RoutedUICommand
       (
           "Dokonaj analizy",
           "Dokonaj analizy",
           typeof(Commands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.Enter)
           }
       );

        public static readonly RoutedUICommand Info = new RoutedUICommand
      (
          "Informacje o aplikacji",
          "Informacje o aplikacji",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.I, ModifierKeys.Control)
          }
      );

    }
}
