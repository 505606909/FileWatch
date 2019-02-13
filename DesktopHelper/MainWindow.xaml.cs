using System.IO;
using System.Windows;

namespace DesktopHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FileSystemWatcher watcher = new FileSystemWatcher();
        }
    }
}
