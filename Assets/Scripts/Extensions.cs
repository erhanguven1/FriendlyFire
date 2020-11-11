using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public static class Extensions
{
    public static T RandomItem<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

	public static T RandomItem<T>()
		where T : Enum
	{
		var enums = Enum.GetValues(typeof(T)).Cast<T>().ToList();
		return enums[Random.Range(0, enums.Count)];
	}
}
