﻿#pragma checksum "C:\Users\Beshoy\Documents\Coptic Hymns 8.0\PanoramaApp1\ItemPage2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CD56640318D9FF01C7DB44550F2E35D1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;


namespace Coptic_Hymns {
    
    
    public partial class ItemPage2 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage pageRoot;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock PopText;
        
        internal System.Windows.Controls.Grid ContentRoot;
        
        internal Telerik.Windows.Controls.RadDataBoundListBox itemListView;
        
        internal Telerik.Windows.Controls.RadBusyIndicator Waiter;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PanoramaApp1;component/ItemPage2.xaml", System.UriKind.Relative));
            this.pageRoot = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("pageRoot")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PopText = ((System.Windows.Controls.TextBlock)(this.FindName("PopText")));
            this.ContentRoot = ((System.Windows.Controls.Grid)(this.FindName("ContentRoot")));
            this.itemListView = ((Telerik.Windows.Controls.RadDataBoundListBox)(this.FindName("itemListView")));
            this.Waiter = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("Waiter")));
        }
    }
}
