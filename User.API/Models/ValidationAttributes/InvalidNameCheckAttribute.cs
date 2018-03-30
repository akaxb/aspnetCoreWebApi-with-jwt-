using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models.ValidationAttributes
{
    /// <summary>
    /// 验证无效的用户名
    /// </summary>
    public class InvalidNameCheckAttribute : ValidationAttribute
    {
        public string _errorName { get; private set; }

        public InvalidNameCheckAttribute(string errorName)
        {
            _errorName = errorName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value?.ToString()==_errorName)
            {
                return new ValidationResult($"{value?.ToString()} can not be use");
            }
            return ValidationResult.Success;
        }
    }
}
