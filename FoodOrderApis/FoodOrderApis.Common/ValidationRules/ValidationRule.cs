using System.Text.RegularExpressions;
using FluentValidation;

namespace FoodOrderApis.Common.ValidationRules;

public static class ValidationRule
{
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder) {

        return ruleBuilder.Must( value => Regex.Match(value, @"^(\d{10})$").Success );
    }
}