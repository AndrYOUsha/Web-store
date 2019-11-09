using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.Validators
{
    public class FloatingPointAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if ((value is float) || (value is double) || (value is decimal))
                return true;

            return false;
        }
    }
}
