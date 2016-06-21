
// LICENSE
// 
// The MIT License (MIT)
// 
// Copyright (c) 2013 Twisted Oak Studios Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without 
// restriction, including without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.

using System.Collections.Generic;
using System;

namespace XOUnityUtils
{
    public class Lens<T>
    {
        public int priority;
        public Func<T, T> transformation;
        public Lens(int priority, Func<T, T> transformation)
        {
            this.priority = priority;
            this.transformation = transformation;
        }
    }

    class LensedValue<T>
    {
        T value;
        List<Lens<T>> lenses;

        public LensedValue(T initialValue)
        {
            this.value = initialValue;
            this.lenses = new List<Lens<T>>();
        }

        public LensToken AddLens(Lens<T> lens)
        {
            lenses.Add(lens);
            return new LensToken(
                () => lenses.Remove(lens)
            );
        }

        public T GetValue()
        {
            T tmp = (T)(value.GetType() == typeof(ICloneable) ? ((ICloneable)value).Clone() : value);
            lenses.Sort((x, y) => x.priority - y.priority);
            foreach (var lens in lenses)
            {
                tmp = lens.transformation(tmp);
            }
            return tmp;
        }
    }

    class LensToken
    {
        Action action;

        public LensToken(Action action)
        {
            this.action = action;
        }

        public void Remove()
        {
            action();
        }
    }
}