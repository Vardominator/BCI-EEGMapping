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
            //bokehHTMLBrowser.Source = new Uri("blah.html");

            // Set up sliders
            batchsizeSlider.IsEnabled = false;

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

                var dataGridSource = BCI_Data_Service.ReadFile(fullPath);

                dataGrid.ItemsSource = dataGridSource.DefaultView;
                
                // Enable batchsizeSlider and ensure that the maximum value corresponds to the table height
                batchsizeSlider.IsEnabled = true;
                batchsizeSlider.Minimum = 0;
                batchsizeSlider.Maximum = dataGrid.Height;

                // Populate Feature Selectors
                foreach (var column in dataGridSource.Columns)
                {
                    horizonalAxisComboBox.Items.Add(column.ToString());
                    verticalAxisComboBox.Items.Add(column.ToString());
                }

                // Initialize neural network creator list box
                layerCreatorListBox.Items.Add($"{dataGridSource.Columns.Count - 1} Nodes @ Input Layer");

            }




        }

        private void clearDataButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
        }

        private void addHiddenLayerButton_Click(object sender, RoutedEventArgs e)
        {
            string hiddenLayerNumber = hiddenLayerNumberTextBox.Text;
            string hiddenLayerNodeCount = hiddenLayerNodeCountTextBox.Text;
            layerCreatorListBox.Items.Add($"{hiddenLayerNodeCount} nodes @ layer {hiddenLayerNumber}");
        }

        private void removeHiddenLayerButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedItemIndex = layerCreatorListBox.SelectedIndex;

            layerCreatorListBox.Items.RemoveAt(selectedItemIndex);
        }

        private void batchsizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            batchsizeTextBox.Text = batchsizeSlider.Value.ToString();
        }
    }
}
