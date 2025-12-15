using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
using System.Runtime.ConstrainedExecution;
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

        public event Action GameOverEvent; 
        public event Action GameWin;

        private int nbToursPerso = 0;
        private DispatcherTimer minuterie;
        private int nb = 0;
        private int nbTourLune = 0;
        private int nbToursPersoAnim = 0;
        private int NbPouvoir = 0 ;

        //Initialisation des images 
        private BitmapImage[] lune = new BitmapImage[5];
        private BitmapImage[] persoAvant = new BitmapImage[8];
        private BitmapImage[] persoDroit = new BitmapImage[6];
        private BitmapImage[] persoGauche = new BitmapImage[6];
        private BitmapImage[] persoArriere = new BitmapImage[6];
        private BitmapImage[] persoCape = new BitmapImage[6];
        private BitmapImage soleil;
        private BitmapImage backgroundJour;
        private BitmapImage castelJour;
        private BitmapImage arbreJour;
        private BitmapImage backgroundNuit;
        private BitmapImage arbreNuit;
        private BitmapImage castelNuitImg;
        private BitmapImage obstacle;
        private BitmapImage cacher;
        private static SoundPlayer plusdecape;
        private static SoundPlayer marcher;
        private static SoundPlayer cape;
        private static SoundPlayer mort;
        private static SoundPlayer win;



        //Initialisation delai de changement des images
        private const int DELAI_LUNE = 30; // initialement pour tester :300 ticks +-= 5 secondes (5*62 img/secondes)
        private const int DELAI_SOLEIL = 120;

        //Initialisation des compteurs pour le delais des images
        private int compteurTickLune = 0;
        private int compteurTickSoleil = 0;


        private bool estSoleil = false;
        private const double POSX_DEPART_LUNE = 760;
        private const double POSY_DEPART_LUNE = 20;
        private const double POSY_CASTEL = 0;
        private const double WIDTH_CASTEL = 338;
        private const double HEIGHT_CASTEL = 452;
        private double posXLune = 760;
        private double vitesseLune = 1.6;  // 1.2 px / tick × 62 = 74 px / seconde
        private int vitesseBackground = 2;
        private Random alea = new Random();

        private int compteurObstacle = 0;
        private const int DELAI_OBSTACLE = 120;


        public UCJeu()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeImages();
            InitSon();
           
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

            //Image de jour
            soleil = new BitmapImage(new Uri("/imgSoleil/Soleil.png", UriKind.Relative));
            castelJour = new BitmapImage(new Uri("/Images_Castel/CastelJour.png", UriKind.Relative));
            backgroundJour = new BitmapImage(new Uri("/imagesBackGround/FOND_JOUR.jpg", UriKind.Relative));
            arbreJour = new BitmapImage(new Uri("/imagesBackGround/arbresJour.png", UriKind.Relative));

            //Image de nuit
            backgroundNuit = new BitmapImage(new Uri("/imagesBackGround/FOND NUIT.png", UriKind.Relative));
            arbreNuit = new BitmapImage(new Uri("/imagesBackGround/arbres.png", UriKind.Relative));
            castelNuitImg = new BitmapImage(new Uri("/Images_Castel/CastelNuit.png", UriKind.Relative));

            //Image des obstacles
            obstacle= new BitmapImage(new Uri("/imgObstables/imgObstacle.png", UriKind.Relative));
            cacher = new BitmapImage(new Uri("/imgObstables/imgCacher.png", UriKind.Relative));

        }

        private void InitSon()
        {
            plusdecape = new SoundPlayer(Application.GetResourceStream(
            new Uri("/Sons/Capeson.wav",UriKind.Relative)).Stream);

            marcher = new SoundPlayer(Application.GetResourceStream(
            new Uri("/Sons/Marcher.wav", UriKind.Relative)).Stream);

            cape = new SoundPlayer(Application.GetResourceStream(
            new Uri("/Sons/Cape.wav", UriKind.Relative)).Stream);

            mort = new SoundPlayer(Application.GetResourceStream(
            new Uri("/Sons/VampireMeurt.wav", UriKind.Relative)).Stream);

            win = new SoundPlayer(Application.GetResourceStream(
            new Uri("/Sons/WinSound.wav", UriKind.Relative)).Stream);
        }


        public void Jeu(object? sender, EventArgs e)
        {
            Annimation_Lune();
            DeplaceImage(imgbackground1, vitesseBackground);
            DeplaceImage(imgbackground2, vitesseBackground);
            VerifCape();
            CreerObjetAleatoire();
            Win();
           
        }
        public void DeplaceImage(Image image, int pas)
        {
            Canvas.SetLeft(image, Canvas.GetLeft(image) - pas);

            if (Canvas.GetLeft(image) + image.Width <= 0)
                Canvas.SetLeft(image, image.ActualWidth);

        }
        private  void Annimation_Lune()
        {
            //Prochaine étape ajouter une valeure de delais aléatoire 

            if (!estSoleil)
            {
                //Compteur tick permet de laisser un delais avant le changement de l'image de la lune
                // la boucle s'execute toute les 16ms ce qui fait 62 img/seconde donc avec un delais = 60 tick cela fait +-= 1 seconde
                compteurTickLune++;
                deplacementLuneLineaire();

                if (compteurTickLune >= DELAI_LUNE)
                {

                    if (nbTourLune < lune.Length)
                    {
                        imgLune1.Source = lune[nbTourLune];

                        //Permet d'afficher 1,2,3 aux images 1,3,5 de la lune
                        switch (nbTourLune)
                        {
                            case 0: 
                                Console.Write("1, ");
                                break;
                            case 2: Console.Write("2, ");
                                break;
                            case 4: Console.Write("3 ");
                                break;
                        }
                        nbTourLune++;
                    }
                        
                    // Affiche le soleil
                    else
                    {
                        AfficheJour();
                        estSoleil = true;
                        compteurTickSoleil = 0;
                    }
                    compteurTickLune = 0;
                }
            }

            // Re-affiche la lune avec un systèreme de delais
            else
            {
                compteurTickSoleil++;
                if (compteurTickSoleil >= DELAI_SOLEIL)
                {
                    AfficheNuit();
                    nbTourLune = 0;
                    estSoleil = false;
                    compteurTickSoleil = 0;
                }
            }

        }


        private bool EstProtegeParlaCape()
        {
            for (int i = 0; i < persoCape.Length; i++)
            {
                if (imgPerso1.Source == persoCape[i])
                    return true;
            }
            return false;
        }

        private void AfficheNuit()
        {
            //Affichage soleil + arbre + background + castel en mode nuit
            imgLune1.Source = lune[0];
            imgLune1.Width = 216;
            imgLune1.Height = 208;
            arbresNuit.Source = arbreNuit;
            imgbackground1.Source = backgroundNuit;
            imgbackground2.Source = backgroundNuit;
            castelNuit.Source = castelNuitImg;
            castelNuit.Width = WIDTH_CASTEL;
            castelNuit.Height = HEIGHT_CASTEL;
            Canvas.SetTop(castelNuit, POSY_CASTEL);
            




        }
        private void AfficheJour()
        {
            Console.Write(": Soleil !");
            //Affichage soleil + arbre + background + castel en mode jour
            imgLune1.Source = soleil;
            imgLune1.Width = 220;
            imgLune1.Height = 220;
            arbresNuit.Source = arbreJour;
            imgbackground1.Source = backgroundJour;
            imgbackground2.Source = backgroundJour;
            castelNuit.Source = castelJour;
            castelNuit.Width = WIDTH_CASTEL;
            castelNuit.Height = 385;
            Canvas.SetTop(castelNuit, POSY_CASTEL);
          




            //Réinitialisation de la position du soleil a l'origine de postion de la lune, cela évite que la lune ce décale a chaque cycle 
            posXLune = POSX_DEPART_LUNE;
            Canvas.SetLeft(imgLune1, POSX_DEPART_LUNE);
            Canvas.SetTop(imgLune1, POSY_DEPART_LUNE);
        }

        private void CreerObjetAleatoire()
        {
            Rectangle rect = new Rectangle();
            rect.Width = 100;
            rect.Height = 100;

            //rect.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/imgObstables/imgObstacle.png", UriKind.Relative)));
            Canvas.SetLeft(rect,400);
            Canvas.SetTop(rect,200);
            ZoneJeu.Children.Add(rect);
        }
        private void GenererObjetsDebutPartie()
        {
         
        }

        //Deplacement de la lune de façon linéaire grace aux calculs d'équations des droites BC et BA.
        private void deplacementLuneLineaire()
        {
            posXLune -= vitesseLune;
            
            double y;
            //Deplacement de B vers A
            if (posXLune >=600)
            {
                //Equation de droite BA
                y = 0.6875 * posXLune - 502.5; //(0.6875 = 11/16)
            }
            //Deplacemenet de B vers C
            else
            {
                //Equation de droite BC
                y = -0.55 * posXLune + 240;
            }

            y=Math.Round(y, 3);
            posXLune= Math.Round(posXLune, 3);
            // Deplacement de la lune sur les fonctions affines
            Canvas.SetLeft(imgLune1, posXLune);
            Canvas.SetTop(imgLune1, y);
            //Console.WriteLine($"position lune x : {posXLune} y :{y}");

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
            GenererObjetsDebutPartie();
        }

        

        private void ZoneJeu_KeyDown(object sender, KeyEventArgs e)
        {

            double PositionX = Canvas.GetLeft(imgPerso1);
            double PositionY = Canvas.GetBottom(imgPerso1);
            //double taille = 1;
            //if (PositionY > 200) { taille = 0.9; }
            //if (PositionY > 300) { taille = 0.8; }        //A QUOI SA SERT ??
            //if (PositionY > 400) { taille = 0.7; }
            Console.WriteLine("X :" + PositionX + " Y : " + PositionY);
            //Vérif si le joueur est a l'interieur des deux fonctions affines avec l'equation de la courbe gauche : y=0.64x+176 et la courbe droite y=-0.67x+1060
            if (PositionY < 0.64 * PositionX + 176 && PositionY < 512 && PositionY < -0.67 * PositionX + 1060)
            {
                Console.WriteLine("Interieur");
                if (e.Key == Key.Z || e.Key == Key.Q || e.Key == Key.S || e.Key == Key.D)
                {
                    if(estSoleil)
                    {
                        GameOver();
                        return;
                    }
                }
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
                    if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) >= 35)
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
                
                if (e.Key == Key.Space && NbPouvoir < 3)
                {
                    cape.Play();
                    for (int i = 0; i < persoCape.Length; i++)
                        persoCape[i] = new BitmapImage(new Uri($"/imgPersoCape/imgPersoCape{i + 1}.png", UriKind.Relative));

                    for (int i = 0;i < persoCape.Length; i++)
                    {
                        
                        imgPerso1.Source = persoCape[i];
                        if (i == 5) { NbPouvoir++; }
                    }
                    
                  
                }
                if (e.Key == Key.Space && NbPouvoir == 3) 
                { 
                    plusdecape.Play();
                }
            }



            else { Console.WriteLine("Exterieur"); Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) - MainWindow.PasVampire); }


        }
        private void VerifCape()
        {
            if (estSoleil && !EstProtegeParlaCape()) // Le joueur meurt si il y a le soleil et qu'il n'a pas sa cape
            {
                GameOver();
                return;
            }
        }
        private void resetGame()
        {
            // Réinitialise la position de la lune
            posXLune = POSX_DEPART_LUNE;
            Canvas.SetLeft(imgLune1, POSX_DEPART_LUNE);
            Canvas.SetTop(imgLune1, POSY_DEPART_LUNE);

            // Réinitialise l'état jour/nuit
            estSoleil = false;
            nbTourLune = 0;
            compteurTickLune = 0;
            compteurTickSoleil = 0;

            // Réinitialise le personnage
            Canvas.SetLeft(imgPerso1, 0);
            Canvas.SetBottom(imgPerso1, 0);
            nb = 0;
            nbToursPerso = 0;

            // Remet les images de fond et lune
            AfficheNuit();

            //Réinitialise les variables
            NbPouvoir = 0;
        }

        private void Win()
        {
            if (Canvas.GetBottom(imgPerso1) > 506) // 506 c'est la postion y du chatêau, ou lorsque le joueur va rentrer en collision contre il gange
            {
                Console.WriteLine("Vous avez gagnez !");
                minuterie.Stop();
                GameWin?.Invoke();
                win.Play();
            }
        }
        private void GameOver()
        {
            mort.Play();
            Console.WriteLine("Vous avez perdu !");
            minuterie.Stop();
            GameOverEvent?.Invoke(); //Déclenche l'évenement et prévient la fenêtre main window que le jeu est terminé  
            resetGame();
        }
    }
}
