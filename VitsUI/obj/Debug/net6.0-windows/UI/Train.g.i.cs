﻿#pragma checksum "..\..\..\..\UI\Train.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B7FB4E8A1F78EBFC64714CA33BDE77315F36F770"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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
using VitsUI.UI;


namespace VitsUI.UI {
    
    
    /// <summary>
    /// Train
    /// </summary>
    public partial class Train : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\UI\Train.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OutPutLogs;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\UI\Train.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox EncoderName;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\UI\Train.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox F0_Index;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VitsUI;component/ui/train.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UI\Train.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.OutPutLogs = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\..\UI\Train.xaml"
            this.OutPutLogs.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Logs_Update);
            
            #line default
            #line hidden
            return;
            case 2:
            this.EncoderName = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.F0_Index = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            
            #line 54 "..\..\..\..\UI\Train.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SampleData_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 62 "..\..\..\..\UI\Train.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Train_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 66 "..\..\..\..\UI\Train.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FeatureTrain_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 70 "..\..\..\..\UI\Train.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DiffTrain_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 74 "..\..\..\..\UI\Train.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ViewLogs_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
