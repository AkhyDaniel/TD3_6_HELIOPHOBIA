using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace SaeProjetGitHubJEU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer minuterie;
        public static int PasVampire { get; set; } = 15;
        public static int PasLune { get; set; } = 2;
        public static string Perso { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
          
        }
        public void AfficheDemarrage()
        {
          UCDemarrage uc = new UCDemarrage();
            ZoneLobby.Content = uc;
            uc.butJouer.Click += ReglesJeu;
            //uc.butParametre.Click += Parametre;
        }

        public void ReglesJeu(object sender, RoutedEventArgs e)
        {
            UCRegle uc = new UCRegle();
            ZoneLobby.Content = uc;
            uc.butJouerRegle.Click += Jeu;
        }
        public void Jeu(object sender, RoutedEventArgs e)
        {
            UCJeu jeu = new UCJeu();
            jeu.GameOverEvent += AfficherGameOver; // Si il y a l'evenement de game over alors cela passe sur UCGameOver
            jeu.GameWin += AfficheEcranWin;        
            ZoneLobby.Content = jeu;

        }
        public void AfficherGameOver()
        {
            ZoneLobby.Content = new UCGameOver();
        }

        public void AfficheEcranWin()
        {
            ZoneLobby.Content = new UCWin();
        }
    } 
}