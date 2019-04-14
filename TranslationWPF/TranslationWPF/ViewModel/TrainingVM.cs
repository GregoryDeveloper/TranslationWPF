using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TranslationWPF.Helper;
using TranslationWPF.Model;
using TranslationWPF.Views;

namespace TranslationWPF.ViewModel
{
    public class TrainingVM:INotifyPropertyChanged
    {
        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        ResourceManager rm;
        CultureInfo ci;

        private ObservableCollection<TranslationVM> Translations = new ObservableCollection<TranslationVM>();
        private TranslationVM selectedItem;
        public TranslationVM SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        #region Properties UI
        private string validateButtonUI;
        public string ValidateButtonUI
        {
            get { return validateButtonUI = (validateButtonUI == null) ? rm.GetString("validateButtonUI", ci) : validateButtonUI; }
        }
        private string previousButtonUI;
        public string PreviousButtonUI
        {
            get { return previousButtonUI = (previousButtonUI == null) ? rm.GetString("previousButtonUI", ci) : previousButtonUI; }
        }

        private string nextButtonUI;
        public string NextButtonUI
        {
            get { return nextButtonUI = (nextButtonUI == null) ? rm.GetString("nextButtonUI", ci) : nextButtonUI; }
        }
        #endregion

        public TrainingVM(List<Translation> translations, ResourceManager rm, CultureInfo ci)
        {
            Translations = ConvertionHelper.ConvertTo(translations);
            SelectedItem = Translations[0];
            this.rm = rm;
            this.ci = ci;
        }

        #region Command
        private CommandHandler nextCommand;
        public CommandHandler NextCommand
        {
            get { return nextCommand ?? (nextCommand = new CommandHandler(() => NextElementHandler(), true)); }

        }

        private CommandHandler previousCommand;
        public CommandHandler PreviousCommand
        {
            get { return previousCommand ?? (previousCommand = new CommandHandler(() => PreviousElementHandler(), true)); }

        }

        private CommandHandler validateCommand;
        public CommandHandler ValidateCommand
        {
            get { return validateCommand ?? (validateCommand = new CommandHandler(() => ValidateElementHandler(), true)); }

        }
        #endregion

        private void PreviousElementHandler()
        {
            if (SelectedItem == Translations.First() )
                return;

            SelectedItem = Translations.Where(t => t.Id == SelectedItem.Id - 1).First();
            
        }

        private void NextElementHandler()
        {
            if (SelectedItem == Translations.Last())
            {
                MessageBoxResult result;
                result = MessageBox.Show(rm.GetString("submitFormMessage"), rm.GetString("submitForm"), MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    ResultView view = new ResultView();
                    view.DataContext = new ResultsVM(Translations);
                    view.ShowDialog();

                    foreach (var t in Translations)
                    {
                        t.Training.Refresh();
                    }

                    SelectedItem = Translations.First();
                    //this.Close();
                }
               
            }

            else
                SelectedItem = Translations.Where(t => t.Id == SelectedItem.Id + 1).First();
        }

        private void ValidateElementHandler()
        {
            if (SelectedItem.Training.Input == SelectedItem.Language2.Value)
            {
                SelectedItem.Training.Found = true;
                SelectedItem.Training.HasTried = true;
                NextElementHandler();
            }
            else
            {
                SelectedItem.Training.Found = false;
                SelectedItem.Training.MistakesCount++;
                SelectedItem.Training.HasTried = true;
            }
        }

    }
}
