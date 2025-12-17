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
        private UCJeu jeuActuel;

        private DispatcherTimer minuterie;
        public static int PasVampire { get; set; } = 6;
        public static int PasLune { get; set; } = 2;    
        public static string Perso { get; set; }

        public static int NbPouvoir { get; set; } = 0;

        public static int NbCapes { get; set; } = 3;

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
            uc.butParametre.Click += (s, e) => AfficheParametre();
            uc.butCheat.Click += (s, e) => AfficheCheat();
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
            // Nettoie l'ancienne instance si elle existe
            if (jeuActuel != null)
            {
                jeuActuel.ResetKeyDownBUG();
            }

            // Crée la nouvelle instance
            jeuActuel = new UCJeu();
            jeuActuel.GameOverEvent += AfficherGameOver;
            jeuActuel.GameWin += AfficheEcranWin;
            ZoneLobby.Content = jeuActuel;
        }
        public void AfficherGameOver()
        {
            ZoneLobby.Content = new UCGameOver();
        }

        public void AfficheEcranWin()
        {
            ZoneLobby.Content = new UCWin();
        }

        public void AfficheParametre()
        {
            Parametre parametre = new Parametre();
            parametre.ShowDialog();
        }

        public void AfficheCheat()
        {
            Cheats cheats = new Cheats();
            cheats.ShowDialog();
        }


        private void cheatmenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void parametremenu_Click(object sender, RoutedEventArgs e)
        {

        }


    } 
}