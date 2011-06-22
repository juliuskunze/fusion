#region Copyright and License Information

// Fluent Ribbon Control Suite
// http://fluent.codeplex.com/
// Copyright © Degtyarev Daniel, Rikker Serg. 2009-2010.  All rights reserved.
// 
// Distributed under the terms of the Microsoft Public License (Ms-PL). 
// The license is available online http://fluent.codeplex.com/license

#endregion

using System.Windows;

namespace Fluent.Sample.ScreenTips
{
    /// <summary>
    /// Entry point of the application
    /// </summary>
    public partial class Application : System.Windows.Application
    {
        /// <summary>
        /// Handles application startup
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        void OnStartup(object sender, StartupEventArgs e)
        {
            ScreenTip.HelpPressed += OnScreenTipHelpPressed;
        }

        /// <summary>
        /// Handles F1 pressed on ScreenTip with help capability
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        static void OnScreenTipHelpPressed(object sender, ScreenTipHelpEventArgs e)
        {
            // Show help according the given help topic
            // (here just show help topic as string)
            MessageBox.Show(e.HelpTopic.ToString());
        }
    }
}