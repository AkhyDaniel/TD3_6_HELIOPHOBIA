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
using System.Windows.Shapes;

namespace SaeProjetGitHubJEU
{
    /// <summary>
    /// Logique d'interaction pour Cheats.xaml
    /// </summary>
    public partial class Cheats : Window
    {
        public Cheats()
        {
            InitializeComponent();
        }

        private void butAnnuleC_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }

        private void butOkC_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
