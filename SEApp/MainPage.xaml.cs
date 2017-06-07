using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
            ErrorLabel.Visibility = Visibility.Collapsed;
            ErrorPathLabel.Visibility = Visibility.Collapsed;
            DirPathBox.IsEnabled = false;
            BrowseButton.IsEnabled = false;
        }

        private async void CalcButton_Click( object sender, RoutedEventArgs e )
        {
            // Initially hide error label;
            ErrorLabel.Visibility = Visibility.Collapsed;

            // Clear output list view in case any data is present
            if( NumberDisplayCtrl.Items.Count() > 0 )
                NumberDisplayCtrl.ItemsSource = null;

            // Check first if input string is all numerics
            if( !IsStringAllDigits( InputText.Text ) )
            {
                ErrorLabel.Visibility = Visibility.Visible;
                ErrorLabel.Text = "Please enter only numeric digits.";
                
                // Run empty task to update gui
                await Task.Run( () =>
                {
                } );

                return;
            }

            // If all numerics, check if input value is 1e9 or less
            if( Convert.ToDouble( InputText.Text ) > 1e9 )
            {
                ErrorLabel.Visibility = Visibility.Visible;
                ErrorLabel.Text = "Please enter an integer value less than 1e9.";

                // Run empty task to update gui
                await Task.Run( () =>
                {
                } );

                return;
            }

            // Then check if output path is valid
            if( DirCheckBox.IsChecked.Value && m_outputFolder == null )
            {
                ErrorPathLabel.Visibility = Visibility.Visible;
                ErrorPathLabel.Text = "Please specify a valid folder.";
                // Run empty task to update gui
                await Task.Run( () =>
                {
                } );
                return;
            }
            ErrorPathLabel.Visibility = Visibility.Collapsed;

            // Display progress bar and label for calculation
            ProgressBarCtrl.Visibility = Visibility.Visible;
            ProgressBarLabel.Visibility = Visibility.Visible;
            ProgressBarLabel.Text = "Generating Prime Numbers...";

            // Run empty task to update gui
            await Task.Run( () =>
            {
            } );

            // Get the user-input value stored in the text box
            m_inputValue = Convert.ToInt32( InputText.Text );

            // Run the improved async segmented Sieve algorithm using the input value
            List<int> primeNums = m_sieve.ComputePrimesSegmentedAsync( m_inputValue );

            // Display all prime numbers to the display control
            NumberDisplayCtrl.ItemsSource = primeNums;


            // If the DirCheckBox is checked, output data to text file in file path
            if( DirCheckBox.IsChecked.Value )
            {
                ProgressBarLabel.Text = "Writing data to file...";
                string timeStamp = DateTime.Now.ToString( "yyyyMMddHHmmss" );
                string fileName = "outputData_" + InputText.Text + "_" + timeStamp + ".txt";
                StorageFile file = await m_outputFolder.CreateFileAsync( fileName );

                // Instantiate a new stream write, convert int to string, and write out lines
                using( StreamWriter sw = new StreamWriter( file.OpenStreamForWriteAsync().Result ) )
                {
                    string headerLine = "Prime numbers generated up to: " + InputText.Text;
                    await sw.WriteAsync( headerLine );
                    await sw.WriteAsync( Environment.NewLine );

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

        private void InputText_TextChanging( TextBox sender, TextBoxTextChangingEventArgs args )
        {
            // Initially hide error label;
            ErrorLabel.Visibility = Visibility.Collapsed;
            /*
            // If the user is not entering numeric digits, 
            // display message and remove them as they type
            double doubleTemp;
            bool isValidDigit = double.TryParse( sender.Text, out doubleTemp );
            if( !isValidDigit && sender.Text != "" )
            {
                int pos = sender.SelectionStart - 1;
                sender.Text = sender.Text.Remove( pos, 1 );
                sender.SelectionStart = pos;
            }
            */
        }

        public bool IsStringAllDigits( string text )
        {
            foreach( char c in text )
            {
                if( !char.IsDigit( c ) )
                    return false;
            }
            return true;
        }


    }
    
}
