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
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {

        private int nbTours = 0;
        private BitmapImage[] lune = new BitmapImage[5];
        public UCJeu()
        {
            InitializeComponent();
            InitializeImages();
        }

        
            
        private void InitializeImages()
        {
            for (int i = 0; i < lune.Length; i++)
                lune[i] = new BitmapImage(new Uri($"/ImagesLune/Lune{i + 1}.png", UriKind.Relative));
        }

        public void DeplaceImage(Image image, int pas)
        {
            Canvas.SetLeft(image, Canvas.GetLeft(image) - pas);

            if (Canvas.GetLeft(image) + image.Width <= 0)
                Canvas.SetLeft(image, image.ActualWidth);

        }

        public void Jeu(object? sender, EventArgs e)
        {
            
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += CanvasTrapeze_KeyDown;
            //    Application.Current.MainWindow.KeyUp += _KeyUp;
        }

        private void CanvasTrapeze_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Z)
            //{
            //    if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) <= (CanvasTrapeze.ActualHeight - imgPerso1.ActualHeight))
            //        Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire);
            //}

            //if (e.Key == Key.S)
            //{
            //    if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) >= 0)
            //        Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) - MainWindow.PasVampire);
            //}

            //if (e.Key == Key.Q)
            //{
            //    if ((Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire) >= 0)
            //        Canvas.SetLeft(imgPerso1, Canvas.GetLeft(imgPerso1) - MainWindow.PasVampire);
            //}

            //if (e.Key == Key.D)
            //{
            //    if ((Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire) <= (CanvasTrapeze.ActualWidth - imgPerso1.ActualWidth))
            //        Canvas.SetLeft(imgPerso1, Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire);
            //}

            // 1. Déclaration et calcul de la nouvelle position
            double pas = MainWindow.PasVampire;

            // Nouvelle position initiale (garder l'ancienne position par défaut)
            double newLeft = Canvas.GetLeft(imgPerso1);
            double newBottom = Canvas.GetBottom(imgPerso1);
            bool mouvementTente = true;

            // Détermination de la Nouvelle Position Tentée
            if (e.Key == Key.Z) // Avancer (Monter) : Bottom AUGMENTE
            {
                newBottom += pas;
            }
            else if (e.Key == Key.S) // Reculer (Descendre) : Bottom DIMINUE
            {
                newBottom -= pas;
            }
            else if (e.Key == Key.Q) // Gauche : Left DIMINUE
            {
                newLeft -= pas;
            }
            else if (e.Key == Key.D) // Droite : Left AUGMENTE
            {
                newLeft += pas;
            }
            else
            {
                mouvementTente = false;
            }

            if (mouvementTente)
            {
                // 2. Conversion Bottom vers Top (nécessaire pour la vérification PathGeometry)
                PathGeometry geometrieLimite = this.FindResource("TrapezeGeometry") as PathGeometry;

                if (geometrieLimite != null)
                {
                    // Conversion de Bottom en Top
                    double newTop = CanvasTrapeze.ActualHeight - newBottom - imgPerso1.ActualHeight;

                    // Point de test : Le centre du personnage à la nouvelle position
                    double centreX = newLeft + (imgPerso1.ActualWidth / 2);
                    double centreY = newTop + (imgPerso1.ActualHeight / 2);

                    Point pointATester = new Point(centreX, centreY);

                    // 3. LA VÉRIFICATION CRITIQUE POUR LE BLOCAGE :
                    if (geometrieLimite.FillContains(pointATester))
                    {
                        // Si le centre est DANS le trapèze, appliquer le mouvement
                        Canvas.SetLeft(imgPerso1, newLeft);
                        Canvas.SetBottom(imgPerso1, newBottom);
                    }
                    // SINON, le mouvement est ignoré. Le personnage reste bloqué à l'ancienne position.
                }
            }


        }
    }
}
