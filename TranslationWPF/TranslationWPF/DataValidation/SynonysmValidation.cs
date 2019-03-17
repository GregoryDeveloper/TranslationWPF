using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TranslationWPF.Helper;

namespace TranslationWPF.DataValidation
{
    public class SynonysmValidation : ValidationRule
    {



        ResourceManager rm;
        CultureInfo ci;
        public int minCharNumber = 1;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result;
            rm = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());
            ci = cultureInfo;
            if ((result = GetWordValidation(value)) != ValidationResult.ValidResult)
                return result;
            if (Exists(value as string))
                return new ValidationResult(false, rm.GetString("validationExistingStringErrorMessage", ci));
            return result;
        }

        ValidationResult GetWordValidation(object word)
        {
            switch (ValidationHelper.IsValid(word, minCharNumber, 999))
            {
                case ValidationHelper.ErrorCode.TypeError:
                    return new ValidationResult(false, rm.GetString("validationInvalidStringErrorMessage", ci));
                case ValidationHelper.ErrorCode.EmptyStringError:
                    return new ValidationResult(false, rm.GetString("validationEmptyStringErrorMessage", ci));
                case ValidationHelper.ErrorCode.FormatStringError:
                    return new ValidationResult(false, rm.GetString("validationFormatStringErrorMessage", ci));
                case ValidationHelper.ErrorCode.LengthStringError:
                    return new ValidationResult(false,
                                        rm.GetString("validationLengthStringErrorMessage1", ci)
                                        + minCharNumber + rm.GetString("validationLengthStringErrorMessage2", ci));
                case ValidationHelper.ErrorCode.OK:
                    return ValidationResult.ValidResult;
                default:
                    return ValidationResult.ValidResult;
            }
        }
        bool Exists(string word)
        {
            throw new NotImplementedException();
            //return Words.Exists(w => w == word);
        }
    }
}
