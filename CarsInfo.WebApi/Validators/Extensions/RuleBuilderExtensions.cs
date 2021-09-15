using System;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace CarsInfo.WebApi.Validators.Extensions
{
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Defines an URL validator on the current rule builder for string properties.
        /// Validation will fail if the value returned by the lambda is not a valid URL.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UrlValidator<T>());

        }
    }
    
    public class UrlValidator<T> : PropertyValidator<T, string>, IRegularExpressionValidator
    { 
        private const string ExpressionPattern = 
            @"(https?:\/\/)?([\w\-])+\.{1}([a-zA-Z]{2,63})([\/\w-]*)*\/?\??([^#\n\r]*)?#?([^\n\r]*)";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            return value is not null && Regex.IsMatch(value);
        }

        public override string Name => "UrlValidator";

        public string Expression => ExpressionPattern;
        
        private static Regex Regex
        {
            get
            {
                const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
                return new Regex(ExpressionPattern, options, TimeSpan.FromSeconds(2));
            }
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return Localized(errorCode, Name);
        }
    }
}
