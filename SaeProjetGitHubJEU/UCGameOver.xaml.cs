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
        public bool rejouer = false;
        public UCGameOver()
        {
            InitializeComponent();
        }

        private void butRejouer_Click(object sender, RoutedEventArgs e)
        {
            rejouer = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).AfficheDemarrage();  // Retourne au menu principal
        }
    }
}
