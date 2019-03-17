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
                oTranslations.Add(new TranslationVM()
                {
                    Translation = t,
                    Language1 = t.Languages[0],
                    Language2 = t.Languages[1],
                });
            }
            return oTranslations;
        }

    }
}
