﻿#pragma checksum "..\..\Datenändern.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FA1184D007C59FDC83B9B9A5A83CA1BE76804870AE1FEF33BA0264B486D4BFBF"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Kassa;
using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
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


namespace Kassa {
    
    
    /// <summary>
    /// Datenändern
    /// </summary>
    public partial class Datenändern : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 40 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock vorname;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbvorname;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button vornameändern;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock nachname;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbnachname;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nachnameändern;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock passwort;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbpasswort;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button passwortändern;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock errorms;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\Datenändern.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button schliesen;
        
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
            System.Uri resourceLocater = new System.Uri("/Kassa;component/daten%c3%a4ndern.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Datenändern.xaml"
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
            this.vorname = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.tbvorname = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\Datenändern.xaml"
            this.tbvorname.KeyDown += new System.Windows.Input.KeyEventHandler(this.tbvorname_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.vornameändern = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\Datenändern.xaml"
            this.vornameändern.Click += new System.Windows.RoutedEventHandler(this.vornameändern_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.nachname = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tbnachname = ((System.Windows.Controls.TextBox)(target));
            
            #line 58 "..\..\Datenändern.xaml"
            this.tbnachname.KeyDown += new System.Windows.Input.KeyEventHandler(this.tbnachname_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.nachnameändern = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\Datenändern.xaml"
            this.nachnameändern.Click += new System.Windows.RoutedEventHandler(this.nachnameändern_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.passwort = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.tbpasswort = ((System.Windows.Controls.TextBox)(target));
            
            #line 73 "..\..\Datenändern.xaml"
            this.tbpasswort.KeyDown += new System.Windows.Input.KeyEventHandler(this.tbpasswort_KeyDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.passwortändern = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\Datenändern.xaml"
            this.passwortändern.Click += new System.Windows.RoutedEventHandler(this.passwortändern_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.errorms = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.schliesen = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\Datenändern.xaml"
            this.schliesen.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

