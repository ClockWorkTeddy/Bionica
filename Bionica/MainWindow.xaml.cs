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

namespace Bionica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel DC = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = DC;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DC.Next();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DC.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DC.Revert();
        }
        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
                DC.Start();
            else if (e.Key == Key.N)
                DC.Next();
        }
    }
}
