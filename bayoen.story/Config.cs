using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.story
{
    public class Config
    {
        public static readonly string PPTName = "puyopuyotetris";
        public static readonly AssemblyName ProjectAssemply = Assembly.GetExecutingAssembly().GetName();
        public static readonly TimeSpan MainTimeSpan = new TimeSpan(0, 0, 0, 0, 5); // 5 ms
    }
}