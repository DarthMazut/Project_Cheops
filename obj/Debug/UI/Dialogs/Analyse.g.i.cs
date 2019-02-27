﻿#pragma checksum "..\..\..\..\UI\Dialogs\Analyse.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1884E6B3382FE90330A88731B20E2331"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Cheops.ChessBoardComponent;
using Cheops.UI.Dialogs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Cheops.UI.Dialogs {
    
    
    /// <summary>
    /// Analyse
    /// </summary>
    public partial class Analyse : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Cheops.ChessBoardComponent.SmartBoard xe_Board;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox xe_DDList;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox xe_checkBox;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox xe_Depth_TextBox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\UI\Dialogs\Analyse.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button xe_Button_OK;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Cheops;component/ui/dialogs/analyse.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UI\Dialogs\Analyse.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.xe_Board = ((Cheops.ChessBoardComponent.SmartBoard)(target));
            return;
            case 2:
            this.textBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.xe_DDList = ((System.Windows.Controls.ComboBox)(target));
            
            #line 18 "..\..\..\..\UI\Dialogs\Analyse.xaml"
            this.xe_DDList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.xe_DDList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.xe_checkBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.xe_Depth_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\..\..\UI\Dialogs\Analyse.xaml"
            this.xe_Depth_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.xe_Depth_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.xe_Button_OK = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\UI\Dialogs\Analyse.xaml"
            this.xe_Button_OK.Click += new System.Windows.RoutedEventHandler(this.xe_Button_OK_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
