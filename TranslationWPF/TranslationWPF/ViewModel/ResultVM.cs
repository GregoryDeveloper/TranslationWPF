
namespace TranslationWPF.ViewModel
{
    public class ResultVM
    {
        public TrainingVM Training { get; set; }
        public string CorrectValues { get; set; } = "";

        public ResultVM(TrainingVM training)
        {
            Training = training;
            CorrectValues = Training.GetRightValues();
        }

       
    }
}
