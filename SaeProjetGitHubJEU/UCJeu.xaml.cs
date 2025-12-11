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
                lune[i] = new BitmapImage(new Uri($"/imagesLunes/DebutLuneDroite{i + 1}.png", UriKind.Relative));
        }

        public void DeplaceImage(Image image, int pas)
        {
            Canvas.SetLeft(image, Canvas.GetLeft(image) - pas);

            if (Canvas.GetLeft(image) + image.Width <= 0)
                Canvas.SetLeft(image, image.ActualWidth);

        }

        public void Jeu(object? sender, EventArgs e)
        {
            DeplaceImage(imgLuneCroissantGauche, 2);
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += CanvasTrapeze_KeyDown;
            //    Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;
        }

        private void CanvasTrapeze_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z)
            {
                if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) <= (CanvasTrapeze.ActualHeight - imgPerso1.ActualHeight))
                    Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire);
            }

            if (e.Key == Key.S)
            {
                if ((Canvas.GetBottom(imgPerso1) + MainWindow.PasVampire) >= 0)
                    Canvas.SetBottom(imgPerso1, Canvas.GetBottom(imgPerso1) - MainWindow.PasVampire);
            }

            if (e.Key == Key.Q)
            {
                if ((Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire) <= 0)
                    Canvas.SetLeft(imgPerso1, Canvas.GetLeft(imgPerso1) - MainWindow.PasVampire);
            }

            if (e.Key == Key.D)
            {
                if ((Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire) <= (CanvasTrapeze.ActualWidth - imgPerso1.ActualWidth))
                    Canvas.SetLeft(imgPerso1, Canvas.GetLeft(imgPerso1) + MainWindow.PasVampire);
            }


        }
    }
}
