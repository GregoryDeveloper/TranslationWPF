using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows;
using TranslationWPF.Exceptions;
using TranslationWPF.Languages;
using TranslationWPF.Model;
using TranslationWPF.Services;
using TranslationWPF.Views;

namespace TranslationWPF.ViewModel
{
    public class TrainingsVM:INotifyPropertyChanged
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

        private ObservableCollection<TrainingVM> Trainings = new ObservableCollection<TrainingVM>();
        private TrainingVM selectedItem;
        public TrainingVM SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        #region Properties UI
        private string validateButtonUI;
        public string ValidateButtonUI
        {
            get { return validateButtonUI = (validateButtonUI == null) ? rm.GetString(StringConstant.validateButtonUI, ci) : validateButtonUI; }
        }
        private string previousButtonUI;
        public string PreviousButtonUI
        {
            get { return previousButtonUI = (previousButtonUI == null) ? rm.GetString(StringConstant.previousButtonUI, ci) : previousButtonUI; }
        }

        private string nextButtonUI;
        public string NextButtonUI
        {
            get { return nextButtonUI = (nextButtonUI == null) ? rm.GetString(StringConstant.nextButtonUI, ci) : nextButtonUI; }
        }
        #endregion

        public TrainingsVM(TranslationService translationService, 
                           ResourceManager rm, 
                           CultureInfo ci, 
                           Language.Languages referenceLanguage, 
                           Language.Languages trainedLanguage)
        {
            if (translationService.Translations.Count == 0)
                throw new NoItemException(rm.GetString(StringConstant.noItemExceptionMessage, ci));

            foreach (Translation item in translationService.Translations)
            {
                if (item.HasLanguages(referenceLanguage, trainedLanguage))
                {
                    Trainings.Add(new TrainingVM(item, referenceLanguage, trainedLanguage));
                }
            }

            SelectedItem = Trainings[0];
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
            if (Trainings.Count == 0)
                return;

            if (SelectedItem == Trainings.First() )
                return;

            //SelectedItem = Trainings.Where(t => t.Id == SelectedItem.Id - 1).First();
            SelectedItem = Trainings.TakeWhile(t => t != SelectedItem).Last();

        }

        private void NextElementHandler()
        {
            if (Trainings.Count == 0)
                return;

            if (SelectedItem == Trainings.Last())
            {
                MessageBoxResult result;
                result = MessageBox.Show(rm.GetString(StringConstant.submitFormMessage), 
                                         rm.GetString(StringConstant.submitForm), 
                                         MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    ResultView view = new ResultView();
                    view.DataContext = new ResultsVM(Trainings,rm,ci);
                    view.ShowDialog();
                    Trainings = ((ResultsVM)view.DataContext).Trainings;

                    if (Trainings.Count> 0)
                        SelectedItem = Trainings.First();
                }
               
            }

            else
                SelectedItem = Trainings.SkipWhile(t => t != SelectedItem).Skip(1).FirstOrDefault();
            
        }

        private void ValidateElementHandler()
        {
            if (SelectedItem.Language2.Validate(SelectedItem.Input))
            {
                SelectedItem.Found = true;
                SelectedItem.HasTried = true;
                SelectedItem.HasMistake = false;
                NextElementHandler();
            }
            else
            {
                SelectedItem.Found = false;
                SelectedItem.MistakesCount++;
                SelectedItem.HasTried = true;
            }
        }

    }
}
