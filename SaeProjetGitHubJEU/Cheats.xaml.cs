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
            ChargerValeursActuelles();
        }

        private void ChargerValeursActuelles()
        {
            // Affiche les valeur actuelles dans les sliders au demarrage
            if (MainWindow.PasVampire == 1)
                sliderVitesse.Value = 1;
            else if (MainWindow.PasVampire == 4)
                sliderVitesse.Value = 2;
            else if (MainWindow.PasVampire == 25)
                sliderVitesse.Value = 3;

            if (MainWindow.NbCapes == 0)
                sliderCapes.Value = 1;
            else if (MainWindow.NbCapes == 3)
                sliderCapes.Value = 2;
            else if (MainWindow.NbCapes == 999)
                sliderCapes.Value = 3;
        }

        private void butAnnuleC_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void butOkC_Click(object sender, RoutedEventArgs e)
        {
            
            if (sliderVitesse.Value == 1)
            {
                MainWindow.PasVampire = 1;
            }
            else if (sliderVitesse.Value == 2)
            {
                MainWindow.PasVampire = 4;
            }
            else if (sliderVitesse.Value == 3)
            {
                MainWindow.PasVampire = 25;
            }

            if (sliderCapes.Value == 1)
            {
                MainWindow.NbCapes = 0;
            }
            else if (sliderCapes.Value == 2)
            {
                MainWindow.NbCapes = 3;
            }
            else if (sliderCapes.Value == 3)
            {
                MainWindow.NbCapes = 999;
            }

            DialogResult = true;

        }
    }
}