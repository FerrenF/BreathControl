﻿using System;
using System.Collections.Generic;

namespace BreathControl.Settings.Elements
{
    internal interface IElement
    {
        List<Control> GetControls(string name, Func<object> propertyGetter, Action<object> propertySetter);
        List<Type> SupportedTypes { get; }
    }
}