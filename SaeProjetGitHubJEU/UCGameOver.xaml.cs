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
    /// Logique d'interaction pour UCGameOver.xaml
    /// </summary>
    public partial class UCGameOver : UserControl
    {
        public bool rejouer = true;
        public UCGameOver()
        {
            InitializeComponent();
        }

        private void butRejouer_Click(object sender, RoutedEventArgs e)
        {
            //Récupération de la fenêtre principale
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            // On crée un nouveau jeu
            UCJeu jeu = new UCJeu();

            // Si le joueur perd alors on fait l'appel méthode affiche l'ecran game over qui est dans la main window
            jeu.GameOverEvent += main.AfficherGameOver;

            // On remplace l'écran actuel par le jeu
            main.ZoneLobby.Content = jeu;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).AfficheDemarrage(); //Revient l'ecran de démarage
        }
    }
}
