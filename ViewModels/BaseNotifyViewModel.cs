using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace bobesponja2._0.ViewModels
{
    public class BaseNotifyViewModel : INotifyPropertyChanged
    {

            public event PropertyChangedEventHandler? PropertyChanged;

            public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
    }
}

