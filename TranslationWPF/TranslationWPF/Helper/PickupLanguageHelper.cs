using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using TranslationWPF.Exceptions;
using TranslationWPF.Languages;
using TranslationWPF.Model;
using TranslationWPF.Services;
using TranslationWPF.ViewModel;
using TranslationWPF.Views;

namespace TranslationWPF.Helper
{
    public static class PickupLanguageHelper
    {
        public static List<Language.Languages> PickUpLanguages(TranslationService translationService, ResourceManager rm, CultureInfo ci)
        {
            if (translationService.Translations.Count == 0)
                throw new NoItemException(rm.GetString(StringConstant.noItemExceptionMessage, ci));

            var window = new LanguagePickupWindow();

            PickupVM pickup = new PickupVM(translationService.GetLanguages());
            window.DataContext = pickup;
            window.ShowDialog();
            List<Language.Languages> languages = new List<Language.Languages>();
            languages.Add(pickup.SelectedItem1);
            languages.Add(pickup.SelectedItem2);
            return languages;
        }
    }
}
