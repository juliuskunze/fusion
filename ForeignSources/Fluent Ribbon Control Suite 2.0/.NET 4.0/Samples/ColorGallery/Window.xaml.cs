#region Copyright and License Information

// Fluent Ribbon Control Suite
// http://fluent.codeplex.com/
// Copyright © Degtyarev Daniel, Rikker Serg. 2009-2010.  All rights reserved.
// 
// Distributed under the terms of the Microsoft Public License (Ms-PL). 
// The license is available online http://fluent.codeplex.com/license

#endregion

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fluent.Sample.ColorGallery
{
    /// <summary>
    /// Represents the main window of the application
    /// </summary>
    public partial class Window : RibbonWindow
    {
        Color[] themeColors = null;

        public Color[] ThemeColors
        {
            get
            {
                if (themeColors == null)
                {
                    themeColors = new Color[10];
                    themeColors[0] = Colors.White;
                    themeColors[1] = Colors.Tan;
                    themeColors[2] = Colors.DarkBlue;
                    themeColors[3] = Colors.Red;
                    themeColors[4] = Colors.DarkOliveGreen;
                    themeColors[5] = Colors.Aqua;
                    themeColors[6] = Colors.Orange;
                    themeColors[7] = Colors.Gray; 
                    themeColors[8] = Colors.Yellow;
                    themeColors[9] = Colors.Black;
                }

                return themeColors;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Window()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}