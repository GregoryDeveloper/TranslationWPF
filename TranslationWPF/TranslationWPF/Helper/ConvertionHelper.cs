using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;
using TranslationWPF.ViewModel;

namespace TranslationWPF.Helper
{
    public static class ConvertionHelper
    {
        /// <summary>
        ///  Get the view model list representation for the translations given as input
        /// </summary>
        /// <param name="translations">The list to convert</param>
        public static ObservableCollection<TranslationVM> ConvertTo(List<Translation> translations)
        {
            ObservableCollection<TranslationVM> oTranslations = new ObservableCollection<TranslationVM>();
            foreach (Translation t in translations)
            {
                oTranslations.Add(new TranslationVM(t));
            }
            return oTranslations;
        }

        //public static Translation ConvertTo(TranslationVM translation)
        //{
        //    Translation t = new Translation(translation.Language1, translation.Language2);

        //    return t;
        //}

    }
}
