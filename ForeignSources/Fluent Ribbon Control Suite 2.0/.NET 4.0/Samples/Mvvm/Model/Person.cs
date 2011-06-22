#region Copyright and License Information

// Fluent Ribbon Control Suite
// http://fluent.codeplex.com/
// Copyright © Degtyarev Daniel, Rikker Serg. 2009-2010.  All rights reserved.
// 
// Distributed under the terms of the Microsoft Public License (Ms-PL). 
// The license is available online http://fluent.codeplex.com/license

#endregion

using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fluent.Sample.Mvvm.Model
{
    /// <summary>
    /// Represents person
    /// </summary>
    public class Person : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // Raise PropertyChanged event
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        // Name
        string name = "Untitled";
        // E-mail
        string email;
        // Phone
        string phone;
        // Photo
        ImageSource photo;

        #endregion

        #region Properies

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets e-mail
        /// </summary>
        public string Email
        {
            get { return email; }
            set
            {
                if (email == value) return;
                email = value;
                RaisePropertyChanged("Email");
            }
        }


        /// <summary>
        /// Gets or sets phone
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone == value) return;
                phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        /// <summary>
        /// Gets or sets photo
        /// </summary>
        public ImageSource Photo
        {
            get { return photo; }
            set
            {
                if (photo == value) return;
                photo = value;
                RaisePropertyChanged("Photo");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates new person
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="email">E-mail</param>
        /// <param name="phone">Phone</param>
        /// <param name="photo">Photo</param>
        /// <returns>Person</returns>
        public static Person Create(string name, string email, string phone, ImageSource photo)
        {
            Person person = new Person();
            person.name = name;
            person.email = email;
            person.phone = phone;
            person.photo = photo;
            return person;
        }

        #endregion
    }
}
