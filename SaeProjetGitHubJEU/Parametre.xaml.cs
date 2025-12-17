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
    /// Logique d'interaction pour Parametre.xaml
    /// </summary>
    public partial class Parametre : Window
    {
        public Parametre()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void butOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
