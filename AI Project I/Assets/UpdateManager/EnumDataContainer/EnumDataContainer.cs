using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnumDataContainer<T1, T2>
    where T2 : Enum
{
    [SerializeField] private T1[] content = null;
    [SerializeField] private T2 enumType;

    public T1 this[int i] => content[i];
    public int Lenght => content.Length;
    public T1[] GetContent() => content;
    public T2 GetEnum() => enumType;
}


