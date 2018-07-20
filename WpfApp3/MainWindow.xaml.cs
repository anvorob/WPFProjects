using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Default Constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region onLoad

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem();
                item.Header = drive;
                item.Tag = drive;
                item.Expanded += FolderExpanded;
                item.Items.Add(null);
                FolderView.Items.Add(item);
            }
        }

        #endregion


        private void FolderExpanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            item.Items.Clear();

            var fullPath = (string)item.Tag;

            #region GetDirectory
            var directories = new List<string>();
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);

                
            }
            catch { }
            directories.ForEach(directorPath =>
            {
                var subItem = new TreeViewItem()
                {
                    Header = GetFileFolderName(directorPath),
                    Tag = directorPath
                };
                subItem.Items.Add(null);
                subItem.Expanded += FolderExpanded;
                item.Items.Add(subItem);
            });
            #endregion Get Directories

            #region Get Files

            var files = new List<string>();
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);


            }
            catch { }

            files.ForEach(filePath =>
            {
                var subItem = new TreeViewItem()
                {
                    Header = GetFileFolderName(filePath),
                    Tag = filePath
                };
                
                item.Items.Add(subItem);
            });
            #endregion
        }


        #region Helpers

        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var normalisedPath = path.Replace('/', '\\');
            var lastIndex = normalisedPath.LastIndexOf('\\');
            if (lastIndex <= 0)
                return path;

            return path.Substring(lastIndex + 1);
        }

        #endregion
    }
}
