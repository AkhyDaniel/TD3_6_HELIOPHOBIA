using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
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
using System.Xml.Schema;

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
        public static int vitessepara = 2;
        public static int capespara = 2;
        

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

        private int DELAI_LUNE = 0; // initialement pour tester :300 ticks +-= 5 secondes (5*62 img/secondes)
        private int DELAI_SOLEIL = 0;

        //Initialisation des compteurs pour le delais des images
        private int compteurTickLune = 0;
        private int compteurTickSoleil = 0;


        private bool estSoleil = false;
        private const double POSX_DEPART_LUNE = 760;
        private const double POSY_DEPART_LUNE = 20;
        private const double POSY_CASTEL = 0;
        private const double WIDTH_CASTEL = 338;
        private const double HEIGHT_CASTEL = 452;
        private const double HAUTEUR_MAX = 512;
        private double posXLune = 760;
        private double vitesseLune = 1.6;  // 1.2 px / tick × 62 = 74 px / seconde
        private int vitesseBackground = 2;
        private Random alea = new Random();
        private Random alea2 = new Random();


        public UCJeu()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeImages();
            InitSon();
            MettreAJourAffichageCapes();
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
            obstacle = new BitmapImage(new Uri("pack://application:,,,/SaeProjetGitHubJEU;component/imgObstacles/imgObstacle.png", UriKind.Absolute));
            cacher = new BitmapImage(new Uri("pack://application:,,,/SaeProjetGitHubJEU;component/imgObstacles/imgCacher.png", UriKind.Absolute));

        }

        private void InitSon()
        {
            plusdecape = new SoundPlayer(Application.GetResourceStream(
            new Uri("/Sons/Capeson.wav", UriKind.Relative)).Stream);

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
            VerifColisionObstacle();
            VerifColisionCacher();
            Win();

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += ZoneJeu_KeyDown;
            CreerObjetAleatoire();
        }
        public void DeplaceImage(Image image, int pas)
        {
            Canvas.SetLeft(image, Canvas.GetLeft(image) - pas);

            if (Canvas.GetLeft(image) + image.Width <= 0)
                Canvas.SetLeft(image, image.ActualWidth);

        }
        private void Annimation_Lune()
        {
            //Prochaine étape ajouter une valeure de delais aléatoire 
            DELAI_LUNE = alea.Next(5, 150);

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
                            case 2:
                                Console.Write("2, ");
                                break;
                            case 4:
                                Console.Write("3 ");
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
                DELAI_SOLEIL = alea2.Next(50, 240);
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

        //Deplacement de la lune de façon linéaire grace aux calculs d'équations des droites BC et BA.
        private void deplacementLuneLineaire()
        {
            posXLune -= vitesseLune;

            double y;
            //Deplacement de B vers A
            if (posXLune >= 600)
            {
               y= EquationGaucheLune(posXLune);
            }
            //Deplacemenet de B vers C avec la formule y = ax + b
            else
            {
                y=EquationDroiteLune(posXLune);
            }

            y = Math.Round(y, 3);
            posXLune = Math.Round(posXLune, 3);
            // Deplacement de la lune sur les fonctions affines
            Canvas.SetLeft(imgLune1, posXLune);
            Canvas.SetTop(imgLune1, y);
            //Console.WriteLine($"position lune x : {posXLune} y :{y}");

        }

        private double EquationDroiteLune(double posXLune)
        {
            double y = 0;
            //Equation de droite BC
            y = -0.55 * posXLune + 240;
            return y;
        }
        private double EquationGaucheLune(double posXLune)
        {
            double y = 0;
            //equation de la droite de gauche de la forme y = ax + b
            //Equation de droite BA avec la formule y = ax + b
            y = 0.6875 * posXLune - 502.5; //(0.6875 = 11/16)
            return y;
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
            ZoneObstacles(-118, 1430, 100, 0, 2, obstacle, 120, 180, "OBSTACLE");
            ZoneObstacles(193, 1130, 300, 20, 2, obstacle, 120, 180, "OBSTACLE");// Calculer a partir des fonction affines x = (y-176)/0.64  et y = (y-1060)/-0.67
                                                                                 // ex 193 => (300-176)/0.64 = 193.75 et 1130 =>(300-1060)/-0.67 =1134.32
                                                                                 // Valeure vonlontairement arrondit pour être sur que l'image ne déborde pas sur les bords
            ZoneObstacles(381, 955, 420, 40, 1, obstacle, 120, 180, "OBSTACLE");
            ZoneObstacles(38, 1283, 200, 30, 2, cacher, 200, 150, "CACHE");
            ZoneObstacles(319, 1014, 380, 40, 1, cacher, 200, 150, "CACHE");
        }
        //Prend une valeure aléatoire compris entre la valeure min et max de x.
        //Et ajoute et soustrait la largeure de l'image pour s'assurer que l'image ne dépasse pas du cadre
        private void ZoneObstacles(int posXMin, int posXMax, int posY, int reductionTailleImg, int nbrDeTours, BitmapImage image, int height, double largeur, string type)
        {
            List<double> positionsExistantes = new List<double>(); // stocke uniquement la position X des obstacles

            largeur -= reductionTailleImg;

            for (int i = 0; i < nbrDeTours; i++)
            {
                double xAleatoire;
                int essais = 0;
                bool chevauche = true;

                do
                {
                    xAleatoire = alea.Next(posXMin, posXMax - (int)largeur);

                    // Vérifie chevauchement avec une boucle classique
                    chevauche = false;
                    foreach (double x in positionsExistantes)
                    {
                        if (Math.Abs(x - xAleatoire) < largeur)   // On regarde si la distance absolue des 2 valeurs entre toutes les positions de x et la nouvelle position qui est généré sont < a la largeure de l'image
                                                                  //SI c'est le cas cela veut dire que les images ce chevauche 
                                                                  // exemple : x = 100 ; xAlea = 120  donc Math.Abs(100 - 120) = 20 < 50 donc chevauchement, on recommence 
                        {
                            chevauche = true;
                            break; // sort de la boucle foreach dès qu'un chevauchement est détecté
                        }
                    }

                    essais++;
                    if (essais > 50)
                        break; // pour éviter boucle infinie

                } while (chevauche);


                positionsExistantes.Add(xAleatoire);

                Rectangle rect = new Rectangle();
                rect.Width = largeur;
                rect.Height = height - reductionTailleImg;
                rect.Fill = new ImageBrush(image);
                rect.Tag = type;

                Canvas.SetLeft(rect, xAleatoire);
                Canvas.SetBottom(rect, posY);
                ZoneJeu.Children.Add(rect);
            }
        }

        private bool VerifColisionObstacle()
        {
            //Prend les coordonnées du personnages pour crée un rectangle
            double persoX = Canvas.GetLeft(imgPerso1);
            double persoY = Canvas.GetBottom(imgPerso1);
            double persoWidth = imgPerso1.ActualWidth;
            double persoHeight = imgPerso1.ActualHeight - 45;
            Rect rectPerso = new Rect(persoX, persoY, persoWidth, persoHeight);

            foreach (UIElement element in ZoneJeu.Children)
            {
                if (element is Rectangle rectObstacle)
                {
                    double obstacleX = Canvas.GetLeft(rectObstacle);
                    double obstacleY = Canvas.GetBottom(rectObstacle);
                    double obstacleWidth = rectObstacle.ActualWidth - 40;
                    double obstacleHeight = rectObstacle.ActualHeight;
                    Rect obstacleRect = new Rect(obstacleX, obstacleY, obstacleWidth, obstacleHeight);

                    if (rectPerso.IntersectsWith(obstacleRect))
                    {
                        Console.WriteLine("BLoqué");
                        return true;
                    }
                }

            }
            return false;
        }


        public bool VerifColisionCacher()
        {
            //Prend les coordonnées du personnages pour crée un rectangle
            double persoX = Canvas.GetLeft(imgPerso1);
            double persoBottom = Canvas.GetBottom(imgPerso1);
            double persoWidth = imgPerso1.ActualWidth;

            foreach (UIElement element in ZoneJeu.Children)// Vérif tout les éléments dans la zone de jeu 
            {
                if (element is Rectangle rectCache && rectCache.Tag?.ToString() == "CACHE")//Permet d'identifier si c'est l'obstacle qui permet de se cacher
                {
                    double cacheX = Canvas.GetLeft(rectCache);
                    double cacheBottom = Canvas.GetBottom(rectCache);
                    double cacheWidth = rectCache.Width;
                    double cacheHeight = rectCache.Height;

                    // Vérifier si le joueur est aligné horizontalement avec la cachette
                    bool aligneHorizontalement = persoX + persoWidth > cacheX && persoX < cacheX + cacheWidth;

                    // Le joueur doit être EN DESSOUS (devant) l'obstacle pour bloquer le soleil
                    // Bottom plus petit = plus bas à l'écran = devant l'obstacle
                    bool estDevantObstacle = persoBottom <= cacheBottom && persoBottom >= cacheBottom - 50;

                    if (aligneHorizontalement && estDevantObstacle)
                    {
#if DEBUG
                        Console.WriteLine("Le joueur est caché du soleil !");
#endif
                        return true;
                    }
                }
            }
            return false;
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
        private double EquationDroiteGauche(double positionX)
        {
            double y = 0;
            //equation de la droite de gauche de la forme y = ax + b
            y = 0.64 * positionX + 176;
            return y;
        }
        private double EquationDroiteDroite(double positionX)
        {
            double y = 0;
            //equation de la droite de droite de la forme y = ax + b
            y = -0.67 * positionX + 1060;
            return y;
        }
        private void ZoneJeu_KeyDown(object sender, KeyEventArgs e)
        {
            double PositionX = Canvas.GetLeft(imgPerso1);
            double PositionY = Canvas.GetBottom(imgPerso1);
            double anciennePositionX = Canvas.GetLeft(imgPerso1);
            double anciennePositionY = Canvas.GetBottom(imgPerso1);
            Console.WriteLine("X :" + PositionX + " Y : " + PositionY);
            //Vérif si le joueur est a l'interieur des deux fonctions affines avec l'equation de la courbe gauche : y=0.64x+176 et la courbe droite y=-0.67x+1060
            if (PositionY < EquationDroiteGauche(PositionX) && PositionY < HAUTEUR_MAX && PositionY < EquationDroiteDroite(PositionX))
            {
                if (e.Key == Key.Space && MainWindow.NbPouvoir < MainWindow.NbCapes)
                {
                    cape.Play();
                    for (int i = 0; i < persoCape.Length; i++)
                        persoCape[i] = new BitmapImage(new Uri($"/imgPersoCape/imgPersoCape{i + 1}.png", UriKind.Relative));

                    for (int i = 0; i < persoCape.Length; i++)
                    {

                        imgPerso1.Source = persoCape[i];
                        if (i == 5)
                        {
                            MainWindow.NbPouvoir++;
                            Console.WriteLine($"Nbpouvoir : {MainWindow.NbPouvoir}");
                            MettreAJourAffichageCapes();
                        }
                    }


                }
                if (e.Key == Key.Space && MainWindow.NbPouvoir == MainWindow.NbCapes)
                {
                    plusdecape.Play();
                }
                Console.WriteLine("Interieur");
                if (e.Key == Key.Z || e.Key == Key.Q || e.Key == Key.S || e.Key == Key.D)
                {

                    if (estSoleil)
                    {
                        GameOver();
                        return;
                    }

                    if (e.Key == Key.Z)
                    {
                        if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) <= (ZoneJeu.ActualHeight - imgPerso1.ActualHeight))
                        {
                            Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire);
                            for (int i = 0; i < persoAvant.Length; i++)
                                persoAvant[i] = new BitmapImage(new Uri($"/imgPerso/imgPerso{i + 1}.png", UriKind.Relative));
                            AnimationPerso(persoAvant);
                            marcher.Play();

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
                            marcher.Play();

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
                            marcher.Play();

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
                            marcher.Play();
                        }
                    }
                    if (VerifColisionObstacle())
                    {
                        Canvas.SetLeft(imgPerso1, anciennePositionX);
                        Canvas.SetBottom(imgPerso1, anciennePositionY);
                    }
                }
            }

            else { Console.WriteLine("Exterieur"); Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) - MainWindow.PasVampire); }
        }
        private void VerifCape()
        {
            if (estSoleil && !EstProtegeParlaCape() && !VerifColisionCacher()) // Le joueur meurt si il y a le soleil et qu'il n'a pas sa cape
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
            MainWindow.NbPouvoir = 0;
            MettreAJourAffichageCapes();
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
        private void MettreAJourAffichageCapes()
        {
            int capesRestantes = MainWindow.NbCapes - MainWindow.NbPouvoir;

            txtNbCapes.Text = $"{capesRestantes}";
        }
    }
}
