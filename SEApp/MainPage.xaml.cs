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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int m_inputValue;
        private string m_dirPath;
        private StorageFolder m_outputFolder;
        private SieveMain m_sieve = new SieveMain();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void CalcButton_Click( object sender, RoutedEventArgs e )
        {
            // Get the user-input value stored in the text box
            m_inputValue = Convert.ToInt32( InputText.Text );

            // Run the improved segmented Sieve algorithm using the input value
            List<int> primeNums = m_sieve.ComputePrimesSegmentedAsync( m_inputValue );

            // If the DirCheckBox is checked, output data to text file in file path
            if( DirCheckBox.IsChecked.Value )
            {
                string timeStamp = DateTime.Now.ToString( "yyyyMMddHHmmss" );
                string fileName = "outputData_" + timeStamp + ".txt";
                //StorageFolder folder = await StorageFolder.GetFolderFromPathAsync( m_dirPath );
                StorageFile file = await m_outputFolder.CreateFileAsync( fileName );

                // TODO: improve writer memory performance
                // TODO: improve list conversion performance
                // Convert primeNums list to IEnumerable<String> to write out all lines
                List<string> strData = (from i in primeNums select i.ToString()).ToList();
                primeNums.Clear();
                GC.Collect();
                await FileIO.WriteLinesAsync( file, strData );
            }
        }

        private void TextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
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


    }
}
