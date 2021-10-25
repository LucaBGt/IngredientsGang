using UnityEngine;
using Fungus;
using Sirenix.OdinInspector;
//using RPG.Data;

[CommandInfo("Gameplay",
             "Update Variables",
             "Updates a global variables.")]
[AddComponentMenu("")]
public class UpdateVariables : Command
{
    public bool updateScene;

    public checkType _checkType;

    //public string key;

    //Int
    [ShowIf("_checkType", checkType._int)]
    [Header("Int")]
    public int intVal;
    public operation intOperation;
    public IntAsset intVar;

    //Float
    [ShowIf("_checkType", checkType._float)]
    [Header("Float")]
    public float floatVal;
    public operation floatOperation;

    public FloatAsset floatVar;

    //Bool
    [ShowIf("_checkType", checkType._bool)]
    [Header("Bool")]
    public bool boolVal;
    public BoolAsset boolVar;

    [ShowIf("_checkType", checkType._string)]
    [Header("String")]
    public string stringVal;
    public StringAsset stringVar;

    public override void OnEnter()
    {

        switch (_checkType)
        {
            case checkType._int:
                switch (intOperation)
                {
                    case operation.setEqual:
                        intVar.Value = intVal;
                        break;
                    case operation.add:
                        intVar.Value = intVar.Value + intVal;
                        break;
                    case operation.subtract:
                        intVar.Value = intVar.Value - intVal;
                        break;
                }
                break;
            case checkType._float:
                switch (floatOperation)
                {
                    case operation.setEqual:
                        floatVar.Value = floatVal;
                        break;
                    case operation.add:
                        floatVar.Value = floatVar.Value + floatVal;
                        break;
                    case operation.subtract:
                        floatVar.Value = floatVar.Value - floatVal;
                        break;
                }
                break;
            case checkType._bool:
                boolVar.Value = boolVal;
                break;
            case checkType._string:
                stringVar.Value = stringVal;
                break;
        }

        if (updateScene)
            StaticEvents.updateScene.Invoke();

        Debug.Log("Example command");
        Continue();
    }

    public enum checkType
    {
        _bool, _int, _float, _string
    }

    public enum operation
    {
        setEqual, add, subtract
    }
}