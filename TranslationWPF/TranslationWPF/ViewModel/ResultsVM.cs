using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TranslationWPF.ViewModel
{
    public class ResultsVM
    {
        public ObservableCollection<ResultVM> Results { get; set; } = new ObservableCollection<ResultVM>();
        public ObservableCollection<ResultVM> Trainings { get; set; } = new ObservableCollection<ResultVM>();

        public ResultVM SelectedItem { get; set; }
        public ResultVM SelectedItem2 { get; set; }

        private ObservableCollection<TranslationVM> translations;

        public ResultsVM(ObservableCollection<TranslationVM> translations)
        {
            foreach (TranslationVM item in translations)
            {
                Results.Add(new ResultVM(item));
            }
            this.translations= translations ;
        }

        #region Commands

        private CommandHandler _addElementCommand;
        public CommandHandler AddElementCommand
        {
            get { return _addElementCommand ?? (_addElementCommand = new CommandHandler(() => AddElementHandler(), true)); }
        }

        private CommandHandler _removeElementCommand;
        public CommandHandler RemoveElementCommand
        {
            get { return _removeElementCommand ?? (_removeElementCommand = new CommandHandler(() => RemoveElementHandler(), true)); }
        }

        private CommandHandler _addNotFoundElementCommand;
        public CommandHandler AddNotFoundElementCommand
        {
            get { return _addNotFoundElementCommand ?? (_addNotFoundElementCommand = new CommandHandler(() => AddNotFoundElementHandler(), true)); }
        }

        private CommandHandler _addMistakesElementCommand;
        public CommandHandler AddMistakesElementCommand
        {
            get { return _addMistakesElementCommand ?? (_addMistakesElementCommand = new CommandHandler(() => AddMistakesElementHandler(), true)); }
        }

        private CommandHandler _closeCommand;
        public CommandHandler CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new CommandHandler(() => CloseHandler(), true)); }
        }
        
        #endregion

        #region Methods
        void AddElementHandler()
        {
            if (SelectedItem != null && !Trainings.Any(t => t.Translation.Id == SelectedItem.Translation.Id))
                Trainings.Add(SelectedItem);
        }

        void RemoveElementHandler()
        {
            if (SelectedItem2 != null && Trainings.Any(t => t.Translation.Id == SelectedItem2.Translation.Id))
                Trainings.Remove(SelectedItem2);
        }

        void AddNotFoundElementHandler()
        {
            var result = Results.Where(r => r.Translation.Training.Found != true);
            List<ResultVM> notFoundList = new List<ResultVM>(result);
            AddIfNotExist(notFoundList, Trainings);
        }

        void AddMistakesElementHandler()
        {
            var result = Results.Where(r => r.Translation.Training.HasTried == false || r.Translation.Training.MistakesCount > 0);
            List<ResultVM> notFoundList = new List<ResultVM>(result);
            AddIfNotExist(notFoundList, Trainings);
        }

        void AddIfNotExist(List<ResultVM> elements, ObservableCollection<ResultVM> data)
        {
            foreach (ResultVM item in elements)
            {
                if (!data.Any(d => d.Translation.Id == item.Translation.Id))
                    Trainings.Add(item);
            }
        }

        void CloseHandler()
        {
            translations.Clear();
            foreach (ResultVM item in Trainings)
            {
                TranslationVM translation = Results
                                .Where(r => r.Translation.Id == item.Translation.Id)
                                .Select(r => r.Translation)
                                .First();
                translations.Add(translation);
            }
        }
        #endregion


    }
}
