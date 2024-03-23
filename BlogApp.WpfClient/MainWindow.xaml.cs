using BlogApp.WpfClient.Windows;
using System;
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

namespace BlogApp.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_OpenSelectedWindow(object sender, RoutedEventArgs e)
        {
            if (cb_windowselector.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem) cb_windowselector.SelectedItem;
                switch (selectedItem.Content)
                {
                    case "Blog":
                        new BlogEditorWindow().Show();
                        break;
                    case "Post":
                        new PostEditorWindow().Show();
                        break;
                    case "Comment":
                        new CommentEditorWindow().Show();
                        break;
                    case "Stats":
                        new StatsWindow().Show();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
