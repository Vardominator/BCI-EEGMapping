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

        // File path for loaded dataset
        string datasetFullPath;

        string TENSORFLOWTRAINING = @"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\TensorFlowDNN\tensorflowdnn.py";
        string TENSORFLOWTESTING = @"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\TensorFlowDNN\tensorflowdnntest.py";
        string BOKEHPLOTTING = @"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\Plotting\bokehtest.py";

        int featureCount;
        int classCount;
        string batchSize;
        string steps;
        string hiddenLayers;
        string testsetPercentage;

        bool firstWebpageload = false;

        public MainWindow()
        {
            InitializeComponent();
            
            string path = Directory.GetCurrentDirectory();
            //bokehHTMLBrowser.Source = new Uri("blah.html");

            // Set up sliders
            batchsizeSlider.IsEnabled = false;
            untrainButton.IsEnabled = false;
        }

        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {


            RemoveOldFiles();

            layerCreatorListBox.Items.Clear();
            testrunResultsTextBlock.Text = "";

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv|TEXT Files (*txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if(result == true)
            {

                string fileName = dialog.SafeFileName;
                datasetFullPath = dialog.FileName;
                loadedDataLabel.Content = fileName;

                var dataGridSource = BCI_Data_Service.ReadFile(datasetFullPath);

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
                featureCount = dataGridSource.Columns.Count - 1;
                layerCreatorListBox.Items.Add($"{dataGridSource.Columns.Count - 1} Nodes @ Input Layer");


                classCount = FindNumberOfClasses(dataGridSource);
                layerCreatorListBox.Items.Add($"{classCount} Nodes @ Output Layer");


                removeHiddenLayerButton.IsEnabled = true;
                addHiddenLayerButton.IsEnabled = true;


                //LoadBokehPlot(datasetFullPath);

            }
            
        }

        private void clearDataButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;

            layerCreatorListBox.Items.Clear();
            testrunResultsTextBlock.Text = "...";

            removeHiddenLayerButton.IsEnabled = false;
            addHiddenLayerButton.IsEnabled = false;

            horizonalAxisComboBox.Items.Clear();
            verticalAxisComboBox.Items.Clear();

            loadedDataLabel.Content = "No data loaded";

            RemoveOldFiles();
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

            batchSize = batchsizeTextBox.Text;
            steps = stepsTextBox.Text;

            // Construct hidden layers
            for(int i = 1; i < layerCreatorListBox.Items.Count - 1; i++)
            {
                hiddenLayers += $"{layerCreatorListBox.Items[i].ToString().Split(' ')[0]} ";
            }


            Console.WriteLine(hiddenLayers);
            if(frameworkComboBox.SelectedIndex == 0)
            {

                // Tensorflow
                PythonService python = new PythonService();
                string args = $"{datasetFullPath} {featureCount} {classCount + 1} {batchSize} {steps} -l {hiddenLayers}";
                python.RunCommand(TENSORFLOWTRAINING, args);

                testrunResultsTextBlock.Text = "Neural Network Trained!";

                trainButton.IsEnabled = false;
                untrainButton.IsEnabled = true;

                // Run bokeh plots
                
            }
            else if(frameworkComboBox.SelectedIndex == 1)
            {
                // SKlearn
            }

        }

        private void untrainButton_Click(object sender, RoutedEventArgs e)
        {

            RemoveOldFiles();
            untrainButton.IsEnabled = false;
        }


        private void RemoveOldFiles()
        {
            try
            {
                Directory.Delete(@"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\data\currentmodel", true);
                File.Delete(@"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\data\trainingset.csv");
                File.Delete(@"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\data\testset.csv");
                File.Delete(@"C:\Users\barse\Desktop\Github\BCI-EEGMapping\BCI_EEG_FrontEnd_WPF\BCI_EEG_FrontEnd_WPF\data\accuracy.txt");

                testrunResultsTextBlock.Text = "";
                trainButton.IsEnabled = true;
            }
            catch (Exception exc)
            {

            }
        }

        private void runtestButton_Click(object sender, RoutedEventArgs e)
        {

            testsetPercentage = datasetPercentageTextBox.Text;

            // Tensorflow
            PythonService python = new PythonService();
            string args = $"{datasetFullPath} {testsetPercentage} {featureCount} {classCount + 1} -l {hiddenLayers}";
            python.RunCommand(TENSORFLOWTESTING, args);

            testrunResultsTextBlock.Text = $"Accuracy: {python.LastResult}";

        }


        private void horizonalAxisComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(verticalAxisComboBox.SelectedIndex != -1)
            {
                LoadBokehPlot(datasetFullPath);
            }
        }

        private void verticalAxisComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(horizonalAxisComboBox.SelectedIndex != -1)
            {
                LoadBokehPlot(datasetFullPath);
            }
        }

        private void LoadBokehPlot(string datasetPath)
        {

            string feature1 = horizonalAxisComboBox.SelectedValue.ToString();
            string feature2 = verticalAxisComboBox.SelectedValue.ToString();

            PythonService python = new PythonService();
            string args = $"{datasetPath} {feature1} {feature2}";
            python.RunCommand(BOKEHPLOTTING, args);

            if (firstWebpageload == false)
            {
                bokehHTMLBrowser.Source = new Uri(@"http://localhost/blahblah.html");
                firstWebpageload = true;
            }
            else
            {
                bokehHTMLBrowser.Reload(true);
            }
            
        }
    }
}
