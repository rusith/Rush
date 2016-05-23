﻿#pragma checksum "..\..\..\Windows\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A9B2E5BAAAA422DBB2FB0084A7BEE58"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Rush.Converters;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Rush.Windows {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SourceLocationsComboBox;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddSourceFolderButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SelectAllSourceFoldersButton;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DestinationTextBox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DestinationBrowseButton;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OrderTextBox;
        
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
            System.Uri resourceLocater = new System.Uri("/Rush;component/windows/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.SourceLocationsComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.AddSourceFolderButton = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\Windows\MainWindow.xaml"
            this.AddSourceFolderButton.Click += new System.Windows.RoutedEventHandler(this.OnAddNewSourceFolderButtonClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 18 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnRemoveSelectedSourceFolderButtonClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.SelectAllSourceFoldersButton = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\Windows\MainWindow.xaml"
            this.SelectAllSourceFoldersButton.Click += new System.Windows.RoutedEventHandler(this.OnClearAllSourceFoldersButtonClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DestinationTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.DestinationBrowseButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\Windows\MainWindow.xaml"
            this.DestinationBrowseButton.Click += new System.Windows.RoutedEventHandler(this.OnDestinationBrowseButtonClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.OrderTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

