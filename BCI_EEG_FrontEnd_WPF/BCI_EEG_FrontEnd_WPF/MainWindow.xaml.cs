using System;
using System.IO;
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

namespace BCI_EEG_FrontEnd_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            string path = Directory.GetCurrentDirectory();

        }

        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv|TEXT Files (*txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                string fileName = dialog.SafeFileName;
                string fullPath = dialog.FileName;
                loadedDataLabel.Content = fileName;

                dataGrid.ItemsSource = BCI_Data_Service.ReadFile(fullPath).DefaultView;
            }
        }

        private void clearDataButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
        }
    }
}
