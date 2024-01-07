using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
//using needed for CallerMemberName in OnPropChanged
using System.Runtime.CompilerServices;

namespace CarbonFootprintDesktopApp.Utilities
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //Base Class for all VM classes, provides support for property change notifications
        public event PropertyChangedEventHandler? PropertyChanged;

        /* CallerMember -> optional param, avoid hardcoding the property name in each call
         * Compiler automatically fill in the name of calling member (property name) */

        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
