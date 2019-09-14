using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using TranslationWPF.Languages;
using System.Resources;
using System.Globalization;

namespace TranslationWPF.ViewModel
{
    public class ResultsVM
    {
        #region Properties
        #region UIProperties
        private string UIword;
        public string UIWord { get { return UIword = UIword ?? rm.GetString(StringConstant.word, ci); } }

        private string UIcorrectValues;
        public string UICorrectValues { get { return UIcorrectValues = UIcorrectValues ?? rm.GetString(StringConstant.correctValues, ci); } }

        private string UIvalue;
        public string UIValue { get { return UIvalue = UIvalue ?? rm.GetString(StringConstant.values, ci); } }

        private string UImistakeCount;
        public string UIMistakeCount { get { return UImistakeCount = UImistakeCount ?? rm.GetString(StringConstant.mistakesCount, ci); } }

        private string UIfound;
        public string UIFound { get { return UIfound = UIfound ?? rm.GetString(StringConstant.found, ci); } }

        private string UItranslation;
        public string UITranslation { get { return UItranslation = UItranslation ?? rm.GetString(StringConstant.translation, ci); } }

        private string UIaddNotFound;
        public string UIAddNotFound { get { return UIaddNotFound = UIaddNotFound ?? rm.GetString(StringConstant.addSome,ci) + "\n" + rm.GetString(StringConstant.notFound, ci); } }

        private string UIaddIncorrect;
        public string UIAddIncorrect { get { return UIaddIncorrect = UIaddIncorrect ?? rm.GetString(StringConstant.addSome,ci) +"\n"+ rm.GetString(StringConstant.incorrect2, ci); } }

        private string UIclose;
        public string UIClose { get { return UIclose = UIclose ?? rm.GetString(StringConstant.close, ci); } }
        #endregion

        #region ViewModel Properties
        ResourceManager rm;
        CultureInfo ci;

        public ObservableCollection<ResultVM> Results { get; set; } = new ObservableCollection<ResultVM>();
        public ObservableCollection<TrainingVM> Trainings { get; set; } = new ObservableCollection<TrainingVM>();

        public ResultVM SelectedItem { get; set; }
        public TrainingVM SelectedItem2 { get; set; }
        #endregion  
        #endregion

        public ResultsVM(ObservableCollection<TrainingVM> results)
        {
            foreach (TrainingVM item in results)
            {
                Results.Add(new ResultVM(item));
            }

            this.rm = LanguageSingleton.Instance.ResourceManager;
            this.ci = LanguageSingleton.Instance.CultureInfo;
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
            if (SelectedItem != null && !Trainings.Any(t => t == SelectedItem.Training))
                Trainings.Add(SelectedItem.Training);
        }

        void RemoveElementHandler()
        {
            if (SelectedItem2 != null && Trainings.Any(t => t == SelectedItem2))
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
                if (!data.Any(d => d == item.Training))
                    Trainings.Add(item.Training);
            }
        }

        void CloseHandler()
        {

            foreach (var training in Trainings)
            {
                training.Refresh();
            }
        }
        #endregion


    }
}
