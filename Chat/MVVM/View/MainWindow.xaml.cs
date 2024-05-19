using System.Text;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using ChatClient.MVVM.ViewModel;
namespace Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }
        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        // Logica para o botão Minimizar
        private void MinimazeButton(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        // Logica para o botão Maximizar
        private void ExpandButton(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }
        // Logica para o botão Fechar
        private void CloseButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // Logica para o botão de enviar arquivo
        private void openArchive(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? sucess = fileDialog.ShowDialog();
            if (sucess == true)
            {
                string path = fileDialog.FileName;
                SendArchiveChat(path);
            }
        }
        private void SendArchiveChat(string filePath)
        {
            try
            {
                if (viewModel != null && viewModel.Messages != null)
                {
                    viewModel.Messages.Add($"Você enviou um arquivo: {System.IO.Path.GetFileName(filePath)}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar arquivo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}