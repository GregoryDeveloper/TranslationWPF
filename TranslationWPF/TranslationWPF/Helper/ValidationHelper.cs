using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TranslationWPF.Helper
{
    public static class ValidationHelper
    {
        public enum ErrorCode
        {
            TypeError,
            EmptyStringError,
            FormatStringError,
            LengthStringError,
            OK
        }
        public static ErrorCode IsValid(object _value,int MinCharNumber, int MaxCharNumber)
        {
            string value;
            int result;
            try
            {
                value = _value as string;
            }
            catch (Exception)
            {
                return ErrorCode.TypeError;
            }
            if (String.IsNullOrWhiteSpace(value))
                return ErrorCode.EmptyStringError;
            if (int.TryParse(value, out result))
                return ErrorCode.FormatStringError;
            if ((value.Length < MinCharNumber) || (value.Length > MaxCharNumber))
            {
                return ErrorCode.LengthStringError;
            }

            return ErrorCode.OK;
        }
    }
}