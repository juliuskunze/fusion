#region Copyright and License Information

// Fluent Ribbon Control Suite
// http://fluent.codeplex.com/
// Copyright © Degtyarev Daniel, Rikker Serg. 2009-2010.  All rights reserved.
// 
// Distributed under the terms of the Microsoft Public License (Ms-PL). 
// The license is available online http://fluent.codeplex.com/license

#endregion

using System.ComponentModel;
using System.Windows.Input;
using Fluent.Sample.Mvvm.Comands;
using Fluent.Sample.Mvvm.Model;

namespace Fluent.Sample.Mvvm.ViewModels
{
    /// <summary>
    /// Represents main view model
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
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

        // All persons
        readonly PersonCollection persons = PersonCollection.Generate();
        // Current person
        Person current;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets current person
        /// </summary>
        public Person Current
        {
            get { return current; }
            set
            {
                if (current == value) return;
                current = value;
                RaisePropertyChanged("Current");
            }
        }

        /// <summary>
        /// Gets persons
        /// </summary>
        public PersonCollection Persons
        {
            get { return persons; }
        }


        #endregion

        #region Commands

        #region Exit

        RelayCommand exitCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
                }
                return exitCommand;
            }
        }

        #endregion
        
        #region Delete

        RelayCommand deleteCommand;

        /// <summary>
        /// Delete this person
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(
                        x => DeleteCurrentPerson(),
                        x => current != null);
                    
                }
                return deleteCommand;
            }
        }

        // Deletes current person
        void DeleteCurrentPerson()
        {
            if (current == null) return;
            Person deleted = current;

            if (persons.Count != 1)
            {
                int index = persons.IndexOf(deleted);
                Current = persons[index == 0 ? 1 : index - 1];
            }
            else
            {
                Current = null;
                deleteCommand.RaiseCanExecuteChanged();
            }

            persons.Remove(deleted);
            
        }

        #endregion

        #region Create

        RelayCommand createCommand;

        /// <summary>
        /// Delete this person
        /// </summary>
        public ICommand CreateCommand
        {
            get
            {
                if (createCommand == null)
                {
                    createCommand = new RelayCommand(x => CreatePerson());

                }
                return createCommand;
            }
        }

        // Creates person
        void CreatePerson()
        {
            persons.Insert(0, new Person());
            Current = persons[0];
            deleteCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #endregion

        #region Initialization

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            if (Persons.Count > 0) Current = Persons[0];
        }

        #endregion
    }
}