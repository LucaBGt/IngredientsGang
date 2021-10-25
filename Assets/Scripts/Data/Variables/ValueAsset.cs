using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

//[CreateAssetMenu(fileName = "ValueAsset", menuName = "~/Documents/ZME/Games/Unity/Tactical RPG Project/Assets/Scripts/Testing/ValueAsset.cs/ValueAsset", order = 0)]
public abstract class ValueAsset<T> : ScriptableObject
{

    public string _name;
    public string _description;

    [System.NonSerialized]
    private T _value;

    [ShowInInspector]
    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }

}