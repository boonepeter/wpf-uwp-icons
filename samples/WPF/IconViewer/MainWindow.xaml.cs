using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IconViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            var contents = new Dictionary<string, ControlTemplate>();
            var items = Application.Current.Resources;
            if (items.GetType() == typeof(ResourceDictionary))
            {
                var rd = (ResourceDictionary)items;
                foreach (var k in rd.Keys)
                {
                    var o = rd[k];
                    if (o.GetType() == typeof(ControlTemplate))
                    {
                        var cont = (ControlTemplate)o;
                        contents.Add((string)k, cont);
                    }
                }
            }
            Icons.ItemsSource = contents;
        }
    }
}
