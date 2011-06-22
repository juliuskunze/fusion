#region Copyright and License Information

// Fluent Ribbon Control Suite
// http://fluent.codeplex.com/
// Copyright © Degtyarev Daniel, Rikker Serg. 2009-2010.  All rights reserved.
// 
// Distributed under the terms of the Microsoft Public License (Ms-PL). 
// The license is available online http://fluent.codeplex.com/license

#endregion

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fluent.Sample.Galleries
{
    /// <summary>
    /// Represets sample data item
    /// </summary>
    public class SampleDataItem:DependencyObject
    {
        /// <summary>
        /// Gets or sets icon
        /// </summary>
        public ImageSource Icon { get; set; }
        /// <summary>
        /// Gets or sets large icon
        /// </summary>
        public ImageSource IconLarge { get; set; }
        /// <summary>
        /// Gets or sets text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets group name
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Creates new item
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <param name="iconLarge">Large Icon</param>
        /// <param name="text">Text</param>
        /// <param name="group">Group</param>
        /// <returns>Item</returns>
        public static SampleDataItem Create(string icon, string iconLarge, string text, string group)
        {
            SampleDataItem dataItem = new SampleDataItem()
            {
                Icon = new BitmapImage(new Uri(icon, UriKind.Relative)),
                IconLarge = new BitmapImage(new Uri(iconLarge, UriKind.Relative)),
                Text = text,
                Group = group
            };
            return dataItem;
        }
    }


    /// <summary>
    /// Represents the main window of the application
    /// </summary>
    public partial class Window : RibbonWindow
    {
        // Data items
        SampleDataItem[] dataItems;

        /// <summary>
        /// Gets data items (uses as DataContext)
        /// </summary>
        public SampleDataItem[] DataItems
        {
            get
            {
                if (dataItems == null)
                {
                    #region Data Items Generation

                    dataItems = new SampleDataItem[]
                    {
                        SampleDataItem.Create("Images\\Blue.png", "Images\\BlueLarge.png","Blue", "Group A"),
                        SampleDataItem.Create("Images\\Brown.png", "Images\\BrownLarge.png","Brown", "Group A"),
                        SampleDataItem.Create("Images\\Gray.png", "Images\\GrayLarge.png","Gray", "Group A"),
                        SampleDataItem.Create("Images\\Green.png", "Images\\GreenLarge.png","Green", "Group A"),
                        SampleDataItem.Create("Images\\Orange.png", "Images\\OrangeLarge.png","Orange", "Group A"),
                        SampleDataItem.Create("Images\\Pink.png", "Images\\PinkLarge.png","Pink", "Group B"),
                        SampleDataItem.Create("Images\\Red.png", "Images\\RedLarge.png","Red", "Group B"),
                        SampleDataItem.Create("Images\\Yellow.png", "Images\\YellowLarge.png","Yellow", "Group B"),
                    };

                    #endregion
                }
                return dataItems;
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