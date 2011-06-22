#region Copyright and License Information

// Fluent Ribbon Control Suite
// http://fluent.codeplex.com/
// Copyright © Degtyarev Daniel, Rikker Serg. 2009-2010.  All rights reserved.
// 
// Distributed under the terms of the Microsoft Public License (Ms-PL). 
// The license is available online http://fluent.codeplex.com/license

#endregion

namespace Fluent.Sample.Foundation
{
    // ATTENTION: You need use Fluent.RibbonWindow. 
    // RibbonWindow designed to provide proper office-like glass style.
    // RibbonWindow automatically will use special non-DWM style in case of
    // Windows XP or basic Windows 7/Vista theme. 
    // You still can use usual System.Windows.Window

    /// <summary>
    /// Represents the main window of the application
    /// </summary>
    public partial class Window : RibbonWindow
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Window()
        {
            InitializeComponent();
        }
    }
}
