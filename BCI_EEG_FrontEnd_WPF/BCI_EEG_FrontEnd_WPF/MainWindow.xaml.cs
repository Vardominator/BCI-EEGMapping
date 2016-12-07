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
using System.Data;

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
                batchsizeSlider.Maximum = dataGridSource.Rows.Count;
               

                // Populate Feature Selectors
                foreach (var column in dataGridSource.Columns)
                {
                    horizonalAxisComboBox.Items.Add(column.ToString());
                    verticalAxisComboBox.Items.Add(column.ToString());
                }

                // Initialize neural network creator list box
                layerCreatorListBox.Items.Add($"{dataGridSource.Columns.Count - 1} Nodes @ Input Layer");


                int numberOfClasses = FindNumberOfClasses(dataGridSource);
                layerCreatorListBox.Items.Add($"{numberOfClasses} Nodes @ Output Layer");


                removeHiddenLayerButton.IsEnabled = true;
                addHiddenLayerButton.IsEnabled = true;

            }
            
        }

        private void clearDataButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;

            removeHiddenLayerButton.IsEnabled = false;
            addHiddenLayerButton.IsEnabled = false;

            horizonalAxisComboBox.Items.Clear();
            verticalAxisComboBox.Items.Clear();
        }

        private void addHiddenLayerButton_Click(object sender, RoutedEventArgs e)
        {

            string hiddenLayerNumber = hiddenLayerNumberTextBox.Text;
            string hiddenLayerNodeCount = hiddenLayerNodeCountTextBox.Text;

            int indexToAdd = int.Parse(hiddenLayerNumber);

            if (indexToAdd > 0 && indexToAdd < layerCreatorListBox.Items.Count && !hiddenLayerNodeCountTextBox.Equals("0"))
            {
                layerCreatorListBox.Items.Insert(indexToAdd, $"{hiddenLayerNodeCount} Nodes @ Hidden Layer");
            }

        }

        private void removeHiddenLayerButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedItemIndex = layerCreatorListBox.SelectedIndex;

            if (selectedItemIndex > 0 && selectedItemIndex < layerCreatorListBox.Items.Count - 1)
            {
                layerCreatorListBox.Items.RemoveAt(selectedItemIndex);
            }

        }

        private void batchsizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            batchsizeTextBox.Text = batchsizeSlider.Value.ToString();
        }

        private void stepsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            stepsTextBox.Text = stepsSlider.Value.ToString();
        }

        private void learningrateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            learningrateTextBox.Text = learningrateSlider.Value.ToString("0.####");
        }

        // Neural network designer
        private int FindNumberOfClasses(DataTable dataTable)
        {
            int lastColumnIndex = dataTable.Columns.Count - 1;
            int numOfClasses = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                string valueStr = row[lastColumnIndex].ToString();

                if (int.Parse(valueStr) > numOfClasses)
                {
                    numOfClasses = int.Parse(valueStr);
                }
            }

            return numOfClasses + 1;
        }

        private void trainButton_Click(object sender, RoutedEventArgs e)
        {
            if(frameworkComboBox.SelectedIndex == 0)
            {
                // Tensorflow
            }
            else if(frameworkComboBox.SelectedIndex == 1)
            {
                // SKlearn
            }
        }
    }
}
