using System;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        public static IRuleBuilderOptions<T, string> CurrentPassword<T>(this IRuleBuilder<T, string> ruleBuilder)     
        {
            // (?=.*[a-z])  // RequireLowercase
            // (?=.*[A-Z])  // RequireUppercase
            // (?=.*\d) // RequireDigit
            // (?=.*\W]) // RequireNonAlphanumeric
            
            return ruleBuilder
                .Matches(@"(?=.*\d)")
                .WithName("目前密碼")
                .WithMessage("{PropertyName}至少需要一位數字")
                .OverridePropertyName("currentPassword")
                .Matches(@"(?=.*[a-z])")
                .WithName("目前密碼")
                .WithMessage("{PropertyName}至少需要一位小寫字母")
                .OverridePropertyName("currentPassword");     
        }
        
        public static IRuleBuilderOptions<T, string> NewPassword<T>(this IRuleBuilder<T, string> ruleBuilder)     
        {
            // (?=.*[a-z])  // RequireLowercase
            // (?=.*[A-Z])  // RequireUppercase
            // (?=.*\d) // RequireDigit
            // (?=.*\W]) // RequireNonAlphanumeric
            
            return ruleBuilder
                .Matches(@"(?=.*\d)")
                .WithName("新密碼")
                .WithMessage("{PropertyName}至少需要一位數字")
                .OverridePropertyName("newPassword")
                .Matches(@"(?=.*[a-z])")
                .WithName("新密碼")
                .WithMessage("{PropertyName}至少需要一位小寫字母")
                .OverridePropertyName("newPassword");     
        }

        public static IRuleBuilderOptions<T, string> RegistrationNumber<T>(this IRuleBuilder<T, string> ruleBuilder)     
        {
            // (?=.*[a-z])  // RequireLowercase
            // (?=.*[A-Z])  // RequireUppercase
            // (?=.*\d) // RequireDigit
            // (?=.*\W]) // RequireNonAlphanumeric
            
            return ruleBuilder
                .Must(VerifyGUI)
                .WithName("統一編號")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("registrationNumber");     
        }

        private static bool VerifyGUI(string input)
        {
            // 假設統一編號為 A B C D E F G H
            // A - G 為編號, H 為檢查碼
            // A - G 個別乘上特定倍數, 若乘出來的值為二位數則將十位數和個位數相加
            // A x 1
            // B x 2
            // C x 1
            // D x 2
            // E x 1
            // F x 2
            // G x 4
            // H x 1
            // 最後將所有數值加總, 被 10 整除就為正確
            // 若上述演算不正確並且 G 為 7 得話, 再加上 1 被 10 整除也為正確
            
            if (input == null)
            {
                return false;
            }
            
            var regex = new Regex(@"^\d{8}$");
            var match = regex.Match(input);
            if (!match.Success)
            {
                return false;
            }
            
            var idNoArray = input.ToCharArray().Select(c => Convert.ToInt32(c.ToString())).ToArray();
            var weight = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };

            int subSum;     //小和
            var sum = 0;    //總和
            var sumFor7 = 1;
            for (var i = 0; i < idNoArray.Length; i++)
            {
                subSum = idNoArray[i] * weight[i];
                sum += (subSum / 10)   //商數
                       + (subSum % 10);  //餘數                
            }
            if (idNoArray[6] == 7)
            {
                //若第7碼=7，則會出現兩種數值都算對，因此要特別處理。
                sumFor7 = sum + 1;
            }
            return (sum % 10 == 0) || (sumFor7 % 10 == 0);
        }
    }
}