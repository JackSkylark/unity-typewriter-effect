using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ByteBros.Common.Variables
{
    public class Variable<T> : ScriptableObject
    {
        public T Value;
    }
}
