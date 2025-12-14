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
            //Récupération de la fenêtre principale
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            // On crée un nouveau jeu
            UCJeu jeu = new UCJeu();

            // Si le joueur gagne alors on fait l'appel méthode affiche l'ecran de win qui est dans la main window
            jeu.GameWin += main.AfficheEcranWin;

            // On remplace l'écran actuel par le jeu
            main.ZoneLobby.Content = jeu;
        }
    }
}
