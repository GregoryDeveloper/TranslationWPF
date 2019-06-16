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
using System.Windows.Shapes;
using TranslationWPF.ViewModel;

namespace TranslationWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour ImportView.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        public event EventHandler<CustomEventArgs> RaiseCustomEvent;
        public ImportView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            RaiseCustomEvent(this, new CustomEventArgs(((ImportVM)DataContext).TranslationModel));
        }
    }
}
