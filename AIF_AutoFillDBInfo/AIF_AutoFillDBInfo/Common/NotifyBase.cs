using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AIFAutoFillDB.Common
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        private string _explanation;
        private SecurityLevel _level;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Explanation
        {
            get { return _explanation; }
            set
            {
                _explanation = value;
                OnPropertyChanged("Explanation");
            }
        }

        // Stand-alone
        public enum SecurityLevel
        {
            None = 0,
            Poor,   // red
            Medium, // yellow
            Good,   // green
            OptedOut, // greyed out
        }

        public SecurityLevel Level
        {
            get
            {
                return SecurityLevel.None;
            }

            set
            {
                _level = value;
            }
        }

    }
}

