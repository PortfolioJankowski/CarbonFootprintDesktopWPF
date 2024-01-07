using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CarbonFootprintDesktopApp.Utilities
{
    //custom btn control dla nawigacji
    public class Btn : RadioButton
    {
        //statyczny konstruktor wywoływany jest raz gdy klasa jest załadowana po raz pierwszy
        //w tym przypadku statyczny konstruktor służy do zdefiniowania niestandardowego wyglądu dla kontrolki Btn
        static Btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Btn), new FrameworkPropertyMetadata(typeof(Btn)));
        }
    }
}
