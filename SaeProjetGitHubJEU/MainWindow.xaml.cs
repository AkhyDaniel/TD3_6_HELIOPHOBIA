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
        private int nbTours = 0;
        private BitmapImage[] lune = new BitmapImage[5];
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            // configure l'intervalle du Timer :62 images par s
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            // associe l’appel de la méthode Jeu à la fin de la minuterie
            minuterie.Tick += Jeu;
            // lancement du timer
            minuterie.Start();

        }
        private void InitializeImages()
        {
            for (int i = 0; i < lune.Length; i++)
                lune[i] = new BitmapImage(new Uri($"pack://application:,,,/imagesLunes/DebutLuneDroite{i + 1}.gif"));
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
            DeplaceImage(DebutLuneDroite, 5);
            nbTours++;
            DebutLuneDroite.Source= lune[nbTours];

            UCJeu uc = new UCJeu();
            ZoneLobby.Content = uc;

        }

        public void DeplaceImage(Image image, int pas)
        {
            Canvas.SetLeft(image, Canvas.GetLeft(image) - pas);

            if (Canvas.GetLeft(image) + image.Width <= 0)
                Canvas.SetLeft(image, image.ActualWidth);

        }

        /* public void Parametre(object sender, RoutedEventArgs e)
         {
             UCParametre uc = new UCParametre();
             ZoneLobby.Content=uc;
         }*/


    } 
}