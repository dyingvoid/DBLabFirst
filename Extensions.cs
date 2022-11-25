﻿using System.Data.SqlTypes;
using System.Globalization;

namespace DBFirstLab;

public static class Extensions
{
    public static TNewCollection PureForEach<TCollection, TValue, TNewCollection, TNewValue>
        (this TCollection collection, Func<TValue, TNewValue> func)
    where TCollection : ICollection<TValue>, new()
    where TNewCollection : ICollection<TNewValue>, new()
    {
        if (collection == null)
            throw new NullReferenceException();
        if (func == null) 
            throw new NullReferenceException();

        var newCollection = new TNewCollection();
        foreach (var value in collection)
        {
            var newValue = func(value);
            newCollection.Add(newValue);
        }

        return newCollection;
    }

    public static T ToType<T>(this string? item) where T : IParsable<T>
    {
        return T.Parse(item, CultureInfo.InvariantCulture);
    }
    
    public static bool IsEmptyOrWhiteSpace(this string? value)
    {
        return value is "" or " ";
    }

    public static void EnlargeListWithNulls(this List<string?> list, int onSize)
    {
        for (int i = 0; i < onSize; ++i)
        {
            list.Add(null);
        }
    }

    public static void CheckSizeEnlarge<TObj>(this List<string?> properties)
    {
        int numberFieldsProps = Controller.NumberOfFieldsAndProps<TObj>();
        if (properties.Count > numberFieldsProps)
        {
            throw new Exception($"List.Count({properties.Count}) must be less or equal" +
                                $"to {typeof(TObj).FullName} number of fields and properties({numberFieldsProps})");
        }

        properties.EnlargeListWithNulls(numberFieldsProps - properties.Count);
    }
}