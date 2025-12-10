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

namespace SaeProjetGitHubJEU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
        }
    

      public void AfficheDemarrage()
        {
          UCDemarrage uc = new UCDemarrage();
            ZoneLobby.Content = uc;
            uc.butJouer.Click += Jeu;
            uc.butParametre.Click += Parametre;
        }
        
        public void Jeu(object sender, RoutedEventArgs e)
        {
            UCJeu uc = new UCJeu();
            ZoneLobby.Content = uc;
            
        }
        public void Parametre(object sender, RoutedEventArgs e)
        {
            UCParametre uc = new UCParametre();
            ZoneLobby.Content=uc;
        }

      
    } 
}