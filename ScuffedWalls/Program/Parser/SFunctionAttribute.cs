﻿using System;
using System.Linq;

namespace ScuffedWalls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SFunctionAttribute : Attribute 
    {
        public SFunctionAttribute(params string[] name)
        {
            ParserName = name.Select(n => n.ToLower().RemoveWhiteSpace()).ToArray();
            Name = name.First();
        }
        public string[] ParserName;
        public string Name;
    }
}