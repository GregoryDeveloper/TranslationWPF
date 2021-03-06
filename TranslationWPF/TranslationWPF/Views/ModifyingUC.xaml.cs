﻿using System;
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
using TranslationWPF.ViewModel;

namespace TranslationWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour ModifyingUC.xaml
    /// </summary>
    public partial class ModifyingUC : UserControl
    {
        public ModifyingUC()
        {
            InitializeComponent();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ((ModifyWordVM)DataContext).TranslationsModel.Clear();

            foreach (var item in ((ModifyWordVM)DataContext).Translations)
            {
                ((ModifyWordVM)DataContext).TranslationsModel.Add(item.Translation);
            }
        }
    }
}
