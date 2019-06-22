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
using TranslationWPF.Languages;
using TranslationWPF.Model;
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

        public TrainingsVM(List<Translation> translations, ResourceManager rm, CultureInfo ci)
        {
            foreach (Translation item in translations)
            {
                Trainings.Add(new TrainingVM(item,Language.Languages.English));
            }
            //Trainings = ConvertionHelper.ConvertTo(translations);
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
            if (SelectedItem == Trainings.First() )
                return;

            SelectedItem = Trainings.Where(t => t.Id == SelectedItem.Id - 1).First();
            
        }

        private void NextElementHandler()
        {
            if (SelectedItem == Trainings.Last())
            {
                MessageBoxResult result;
                result = MessageBox.Show(rm.GetString("submitFormMessage"), rm.GetString("submitForm"), MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    ResultView view = new ResultView();
                    view.DataContext = new ResultsVM(Trainings,rm,ci);
                    view.ShowDialog();
                    Trainings = ((ResultsVM)view.DataContext).Trainings;

                    foreach (var t in Trainings)
                    {
                        t.Refresh();
                    }
                    if (Trainings.Count> 0)
                        SelectedItem = Trainings.First();
                }
               
            }

            else
                SelectedItem = Trainings.Where(t => t.Id == SelectedItem.Id + 1).First();
        }

        private void ValidateElementHandler()
        {
            if (SelectedItem.Input == SelectedItem.Language2.Value)
            {
                SelectedItem.Found = true;
                SelectedItem.HasTried = true;
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
