using System;
using System.Collections.Generic;
using System.Text;

namespace SkiTurkish.Model
{
    using System;
    [AttributeUsage(AttributeTargets.Property)]
    public class InputTypeAttribute : Attribute
    {
        private string name;

        // Constructor 
        public InputTypeAttribute(string name)
        {
            this.name = name;
        }

        // property to get name 
        public string Name
        {
            get { return name; }
        }
    }
}
