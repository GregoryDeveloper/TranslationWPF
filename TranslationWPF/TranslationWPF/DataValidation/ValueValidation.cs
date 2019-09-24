using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using TranslationWPF.Helper;

namespace TranslationWPF.DataValidation
{
    public class ValueValidation:ValidationRule
    {
        ResourceManager rm;
        private readonly int maxCharNumber = 25;

        public int MinCharNumber { get; set; } = 1;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            rm = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());

            switch (ValidationHelper.IsValid(value,MinCharNumber,maxCharNumber))
            {
                case ValidationHelper.ErrorCode.TypeError:
                    return new ValidationResult(false, rm.GetString("validationInvalidStringErrorMessage", cultureInfo));
                case ValidationHelper.ErrorCode.EmptyStringError:
                    return new ValidationResult(false, rm.GetString("validationEmptyStringErrorMessage", cultureInfo));
                case ValidationHelper.ErrorCode.FormatStringError:
                    return new ValidationResult(false, rm.GetString("validationFormatStringErrorMessage", cultureInfo));
                case ValidationHelper.ErrorCode.LengthStringError:
                    return new ValidationResult(false,
                                        rm.GetString("validationLengthStringErrorMessage1", cultureInfo) 
                                        + MinCharNumber + rm.GetString("validationLengthStringErrorMessage2", cultureInfo));
                case ValidationHelper.ErrorCode.OK:
                    return ValidationResult.ValidResult;
                default:
                    return ValidationResult.ValidResult;
            }            
        }

        public bool IsValid(object word, object translation)
        {
            return ValidationHelper.IsValid(word, MinCharNumber, maxCharNumber) == ValidationHelper.ErrorCode.OK
                && ValidationHelper.IsValid(translation, MinCharNumber, maxCharNumber) == ValidationHelper.ErrorCode.OK;
        }


    }


    public class Properties:DependencyObject
    {
        public ObservableCollection<string> Words { get; set; }
    }


}
