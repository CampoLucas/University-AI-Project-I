using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnumDataContainer<T1, T2>
    where T2 : Enum
{
    [SerializeField] private T1[] content = null;
    [SerializeField] private T2 enumType;

    public T1 this[int i]
    {
        get
        {
            return content[i];
        }
    }

    public int Lenght
    {
        get
        {
            return content.Length;
        }
    }

    public T1[] GetContent()
    {
        return content;
    }

    public T2 GetEnum()
    {
        return enumType;
    }
}


