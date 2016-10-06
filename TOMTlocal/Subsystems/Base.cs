using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOMTlocal.Subsystems
{
    abstract class Base
    {
        public delegate void EventDelegate(bool value);
        public abstract event EventDelegate EventHandler;

        protected bool _Value;

        public abstract bool Value { get; set; }
    }
}
