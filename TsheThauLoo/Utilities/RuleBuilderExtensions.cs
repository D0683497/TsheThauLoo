using System;
using System.Linq;
using FluentValidation;
using PhoneNumbers;

namespace TsheThauLoo.Utilities
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> NationalId<T>(this IRuleBuilder<T, string> ruleBuilder)     
        {
            return ruleBuilder
                .Matches(@"^[A-Z][1,2,8,9]\d{8}$")
                .WithName("身份證字號")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("nationalId")
                .Must(VerifyTaiwanId)
                .WithName("身份證字號")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("nationalId");     
        }

        private static bool VerifyTaiwanId(string input)
        {
            var seed = new int[9];
            var charMapping = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };
            var target = input.Substring(0, 1);
            for (var index = 0; index < charMapping.Length; index++)
            {
                if (charMapping[index] == target)
                {
                    index += 10;
                    var n1 = index / 10;
                    var n2 = index % 10;
                    seed[0] = (n2 * 9 + n1) % 10;
                    break;
                }
            }
            for (var index = 1; index < 9; index++)
            {
                seed[index] = Convert.ToInt32(input.Substring(index, 1)) * (9 - index);
            }
            return (10 - (seed.Sum() % 10)) % 10 == Convert.ToInt32(input.Substring(9, 1));
        }
        
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(VerifyPhoneNumber);
        }

        private static bool VerifyPhoneNumber(string input)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            try
            {
                var phone = phoneNumberUtil.Parse(input, "TW");
                return phoneNumberUtil.IsValidNumberForRegion(phone, "TW");
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
        
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)     
        {
            // (?=.*[a-z])  // RequireLowercase
            // (?=.*[A-Z])  // RequireUppercase
            // (?=.*\d) // RequireDigit
            // (?=.*\W]) // RequireNonAlphanumeric
            
            return ruleBuilder
                .Matches(@"(?=.*\d)")
                .WithName("密碼")
                .WithMessage("{PropertyName}至少需要一位數字")
                .OverridePropertyName("password")
                .Matches(@"(?=.*[a-z])")
                .WithName("密碼")
                .WithMessage("{PropertyName}至少需要一位小寫字母")
                .OverridePropertyName("password");     
        }
    }
}