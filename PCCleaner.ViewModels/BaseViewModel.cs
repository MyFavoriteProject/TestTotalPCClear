using PCCleaner.ViewModels.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCleaner.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int busyCount;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public int BusyCount
        {
            get => this.busyCount;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                var previousIsBusy = this.IsBusy;
                var previousCount = this.busyCount;
                this.busyCount = value;
                this.OnBusyCountChanged(previousCount, value);

                if (previousIsBusy != this.IsBusy)
                {
                    this.OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public bool IsBusy => this.BusyCount != 0;

        protected virtual void OnBusyCountChanged(int previousValue, int newValue)
        {
            var change = newValue - previousValue;
            var message = new BusyCountChangeMessage(change);
            //this.MessengerInstance.Send(message);
        }
    }
}
