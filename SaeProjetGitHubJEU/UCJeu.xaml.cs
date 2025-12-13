using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Threading;

namespace SaeProjetGitHubJEU
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {

        private int nbToursPerso = 0;
        private BitmapImage[] lune = new BitmapImage[5];
        private DispatcherTimer minuterie;
        private int nb = 0;
        private int nbTourLune = 0;
        private BitmapImage[] persoAvant = new BitmapImage[7];
        private BitmapImage[] persoDroit = new BitmapImage[5];
        private BitmapImage[] persoGauche = new BitmapImage[5];
        private BitmapImage[] persoArriere = new BitmapImage[5];
        private const int DELAI_LUNE = 120; // 300 ticks ≈ 5 secondes (5*62 img/secondes)
        private int compteurTickLune = 0;
        private bool estSoleil = false;
        private BitmapImage soleil;

        public UCJeu()
        {

            InitializeComponent();
            InitializeTimer();
            InitializeImages();
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
                lune[i] = new BitmapImage(new Uri($"/ImagesLune/Lune{i + 1}.png", UriKind.Relative));

            soleil = new BitmapImage(new Uri($"/imgSoleil/Soleil.png", UriKind.Relative));

        }

        public void DeplaceImage(Image image, int pas)
        {
            Canvas.SetLeft(image, Canvas.GetLeft(image) - pas);

            if (Canvas.GetLeft(image) + image.Width <= 0)
                Canvas.SetLeft(image, image.ActualWidth);

        }

        public void Jeu(object? sender, EventArgs e)
        {
            Annimation_Lune();
            

        }
            
          
       
        private  void Annimation_Lune()
        {

            if (!estSoleil)
            {
                if (nbTourLune < 3)//lune.Length
                {
                    compteurTickLune++;
                    if (compteurTickLune >= DELAI_LUNE)
                    {
                        imgLune1.Source = lune[nbTourLune];
                        // Le % permet de revenir a 0 lorsqu'on sera a la 5 ème image. car (4+1)%5=0 don on recommence la boucle 
                        nbTourLune = (nbTourLune + 1) % lune.Length;
                        Console.WriteLine("Le nombre de tours:" + nbTourLune);
                        compteurTickLune = 0;
                    }
                }
                else
                {
                    Console.WriteLine("image soleil");
                    imgLune1.Source = soleil;
                    estSoleil = true;
                }
            }

            else
            {
                nbTourLune = 0;
                estSoleil = false;
            }
               
                
            }
        
        
       //Methode pour unifier toutes les annimations du personnage
        private void AnimationPerso(BitmapImage[] ImgPerso)
        {
            nbToursPerso++;
            if (nbToursPerso == 4)
            {
                nb++;
                if (nb >= ImgPerso.Length)
                {
                    nb = 0;
                }
                imgPerso1.Source = ImgPerso[nb];
                nbToursPerso = 0;

                }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += ZoneJeu_KeyDown;
            //    Application.Current.MainWindow.KeyUp += _KeyUp;
        }

        private void ZoneJeu_KeyDown(object sender, KeyEventArgs e)
        {

            double PositionX = Canvas.GetLeft(imgPerso1);
            double PositionY = Canvas.GetBottom(imgPerso1);
            double taille = 1;
            if (PositionY > 200) { taille = 0.9; }
            if (PositionY > 300) { taille = 0.8; }
            if (PositionY > 400) { taille = 0.7; }
            Console.WriteLine("X :" + PositionX + " Y : " + PositionY);
            //Vérif si le joueur est a l'interieur des deux fonctions affines avec l'equation de la courbe gauche : y=0.64x+176 et la courbe droite y=-0.67x+1060
            if (PositionY < 0.64 * PositionX + 176 && PositionY < 512 && PositionY < -0.67 * PositionX + 1060)
            {
                Console.WriteLine("Interieur");
                if (e.Key == Key.Z)
                {
                    if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) <= (ZoneJeu.ActualHeight - imgPerso1.ActualHeight))
                    {
                        Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire);
                        for (int i = 0; i < persoAvant.Length; i++)
                            persoAvant[i] = new BitmapImage(new Uri($"/imgPerso/imgPerso{i + 1}.png", UriKind.Relative));
                        AnimationPerso(persoAvant);
                    }
                       
                }

                if (e.Key == Key.S)
                {
                    if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) >= 0)
                    {
                        Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) - MainWindow.PasVampire);
                        for (int i = 0; i < persoArriere.Length; i++)
                            persoArriere[i] = new BitmapImage(new Uri($"/imgPersoArriere/imgPersoA{i + 1}.png", UriKind.Relative));
                        
                        AnimationPerso(persoArriere);
                    }
                      
                }

                if (e.Key == Key.Q)
                {
                    if ((Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire) >= 0)
                    {
                        Canvas.SetLeft(imgPerso1, Canvas.GetLeft(imgPerso1) - MainWindow.PasVampire);
                        for (int i = 0; i < persoGauche.Length; i++)
                            persoGauche[i] = new BitmapImage(new Uri($"/imgPersoGauche/imgPersoG{i + 1}.png", UriKind.Relative));
                        AnimationPerso(persoGauche);
                    }
                    
                }

                    if (e.Key == Key.D)
                    {
                        if ((Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire) <= (ZoneJeu.ActualWidth - imgPerso1.ActualWidth))
                        {
                            Canvas.SetLeft(imgPerso1, Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire);
                            for (int i = 0; i < persoDroit.Length; i++)
                                persoDroit[i] = new BitmapImage(new Uri($"/imgPersoDroit/imgPersoD{i + 1}.png", UriKind.Relative));

                            AnimationPerso(persoDroit);

                        }
                    }
            }



            else { Console.WriteLine("Exterieur"); Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) - MainWindow.PasVampire); }


        }

      

        private void CanvaObstacleDroit_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
