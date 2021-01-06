using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxCountAttribute : ValidationAttribute
    {
        private readonly int maxCount;

        public MaxCountAttribute(int maxCount) => this.maxCount = maxCount;

        public override bool IsValid(object value) => (value as IList).Count <= maxCount;
    }
}
