using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class ResultsVM
    {
        public ObservableCollection<ResultVM> Results { get; set; } = new ObservableCollection<ResultVM>();
        public ObservableCollection<TrainingVM> Trainings { get; set; } = new ObservableCollection<TrainingVM>();

        public ResultVM SelectedItem { get; set; }
        public TrainingVM SelectedItem2 { get; set; }

        public ResultsVM(ObservableCollection<TrainingVM> results)
        {
            foreach (TrainingVM item in results)
            {
                Results.Add(new ResultVM(item));
            }            
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
            if (SelectedItem != null && !Trainings.Any(t => t.Id == SelectedItem.Training.Id))
                Trainings.Add(SelectedItem.Training);
        }

        void RemoveElementHandler()
        {
            if (SelectedItem2 != null && Trainings.Any(t => t.Id == SelectedItem2.Id))
                Trainings.Remove(SelectedItem2);
        }

        void AddNotFoundElementHandler()
        {
            var result = Results.Where(r => r.Training.Found != true);
            List<ResultVM> notFoundList = new List<ResultVM>(result);
            AddIfNotExist(notFoundList, Trainings);
        }

        void AddMistakesElementHandler()
        {
            var result = Results.Where(r => r.Training.HasTried == false || r.Training.MistakesCount > 0);
            List<ResultVM> notFoundList = new List<ResultVM>(result);
            AddIfNotExist(notFoundList, Trainings);
        }

        void AddIfNotExist(List<ResultVM> elements, ObservableCollection<TrainingVM> data)
        {
            foreach (ResultVM item in elements)
            {
                if (!data.Any(d => d.Id == item.Training.Id))
                    Trainings.Add(item.Training);
            }
        }

        void CloseHandler()
        {

        }
        #endregion


    }
}
