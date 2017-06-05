using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SEApp
{
    /// <summary>
    /// The Main Page of the Sieve App; contains controls to allow
    /// user to input values to generate primes and output data
    /// to text file
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int m_inputValue;
        private string m_dirPath;
        private StorageFolder m_outputFolder;
        private SieveMain m_sieve = new SieveMain();

        public string m_progressLabelText;

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeControls();
        }

        public void InitializeControls()
        {
            ProgressBarCtrl.Visibility = Visibility.Collapsed;
            ProgressBarLabel.Visibility = Visibility.Collapsed;
            DirPathBox.IsEnabled = false;
            BrowseButton.IsEnabled = false;
        }

        private async void CalcButton_Click( object sender, RoutedEventArgs e )
        {
            // Get the user-input value stored in the text box
            // TODO: check if valid input
            m_inputValue = Convert.ToInt32( InputText.Text );

            // Display progress bar and label for calculation
            ProgressBarCtrl.Visibility = Visibility.Visible;
            ProgressBarLabel.Visibility = Visibility.Visible;
            ProgressBarLabel.Text = "Generating Prime Numbers...";

            // Run empty task to update gui
            await Task.Run( () =>
            {
            } );

            // Run the improved async segmented Sieve algorithm using the input value
            List<int> primeNums = m_sieve.ComputePrimesSegmentedAsync( m_inputValue );

            // If the DirCheckBox is checked, output data to text file in file path
            if( DirCheckBox.IsChecked.Value )
            {
                ProgressBarLabel.Text = "Writing data to file...";
                string timeStamp = DateTime.Now.ToString( "yyyyMMddHHmmss" );
                string fileName = "outputData_" + timeStamp + ".txt";
                StorageFile file = await m_outputFolder.CreateFileAsync( fileName );

                // Instantiate a new stream write, convert int to string, and write out lines
                using( StreamWriter sw = new StreamWriter( file.OpenStreamForWriteAsync().Result ) )
                {
                    foreach( int num in primeNums )
                    {
                        string line = num.ToString() + Environment.NewLine;
                        await sw.WriteAsync( line );
                    }
                }
            }

            // Hide progress bar and label when calculation and output is completed
            ProgressBarLabel.Text = "Completed!";
            ProgressBarCtrl.Visibility = Visibility.Collapsed;

            // Run empty task to update gui
            await Task.Run( () =>
            {
            } );
        }

        private void DirCheckBox_Click( object sender, RoutedEventArgs e )
        {
            // If the user has not checked to output the file 
            if( !DirCheckBox.IsChecked.Value )
            {
                // Disable the DirPathBox and the BrowseButton
                DirPathBox.IsEnabled = false;
                BrowseButton.IsEnabled = false;
            }
            else
            {
                // Enable the DirPathBox and the BrowseButton
                DirPathBox.IsEnabled = true;
                BrowseButton.IsEnabled = true;
            }
        }

        // Async method for picking a folder to store the output to

        private async void BrowseButton_Click( object sender, RoutedEventArgs e )
        {
            // Create a file picker
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add( "*" );

            // Get the file from the user
            m_outputFolder = await folderPicker.PickSingleFolderAsync();

            // Store the file path and update the DirPathBox
            if( m_outputFolder != null )
            {
                m_dirPath = m_outputFolder.Path;
                DirPathBox.Text = m_dirPath;
            }
        }

        /*
        private void InputText_KeyDown( object sender, KeyRoutedEventArgs e )
        {
            if( !char.IsControl( e.KeyChar ) && !char.IsDigit( e.KeyChar ) && (e.KeyChar != '.') )
            {
                e.Handled = true;
            }
        }
        */
    }
}
