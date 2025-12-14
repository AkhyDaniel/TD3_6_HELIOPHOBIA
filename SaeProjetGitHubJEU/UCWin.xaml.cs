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

namespace SaeProjetGitHubJEU
{
    /// <summary>
    /// Logique d'interaction pour UCWin.xaml
    /// </summary>
    public partial class UCWin : UserControl
    {
        public UCWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).AfficheDemarrage();
        }

        private void butRejouer_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Jeu(null, null); // Récupère la fenêtre principale de l’application (MainWindow) et lance la méthode Jeu()
                                                                          // comme si le joueur venait de cliquer sur le bouton pour démarrer le jeu.
        }
    }
}
