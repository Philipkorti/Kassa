﻿#pragma checksum "..\..\ProdukteAdd.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "04727DCE7229AAD57B9BB7BDE953DE8094796F001B4005A038B3627D3243E8D6"
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
    /// ProdukteAdd
    /// </summary>
    public partial class ProdukteAdd : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbname;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbpreis;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox preis;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblager;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox lager;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\ProdukteAdd.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock errorms;
        
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
            System.Uri resourceLocater = new System.Uri("/Kassa;component/produkteadd.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ProdukteAdd.xaml"
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
            this.tbname = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.name = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\ProdukteAdd.xaml"
            this.name.GotFocus += new System.Windows.RoutedEventHandler(this.name_GotFocus);
            
            #line default
            #line hidden
            
            #line 28 "..\..\ProdukteAdd.xaml"
            this.name.LostFocus += new System.Windows.RoutedEventHandler(this.name_LostFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbpreis = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.preis = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\ProdukteAdd.xaml"
            this.preis.GotFocus += new System.Windows.RoutedEventHandler(this.preis_GotFocus);
            
            #line default
            #line hidden
            
            #line 31 "..\..\ProdukteAdd.xaml"
            this.preis.LostFocus += new System.Windows.RoutedEventHandler(this.preis_LostFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tblager = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.lager = ((System.Windows.Controls.TextBox)(target));
            
            #line 34 "..\..\ProdukteAdd.xaml"
            this.lager.GotFocus += new System.Windows.RoutedEventHandler(this.lager_GotFocus);
            
            #line default
            #line hidden
            
            #line 34 "..\..\ProdukteAdd.xaml"
            this.lager.LostFocus += new System.Windows.RoutedEventHandler(this.lager_LostFocus);
            
            #line default
            #line hidden
            return;
            case 7:
            this.errorms = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            
            #line 43 "..\..\ProdukteAdd.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 44 "..\..\ProdukteAdd.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

