using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
//using RPG.Data;

namespace RPG.NPC
{
    [System.Serializable]
    public class NPCPage
    {
        public List<PageCheck> checks;

        public GameObject pageObject;

        public bool isActive()
        {
            if (checks.Count == 0)
                return true;

            foreach (PageCheck pc in checks)
            {
                if (!pc.isPageTrue())
                    return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class PageCheck
    {
        public enum checkType
        {
            _bool, _int, _float, _quest, _item
        }

        public checkType _checkType;

        [ShowIf("@this._checkType != checkType._bool")]
        public valueCheck valueCheck;

        //Int
        [ShowIf("_checkType", checkType._int)]
        public IntAsset intRef;
        [ShowIf("_checkType", checkType._int)]
        public int intVal;

        //Float
        [ShowIf("_checkType", checkType._float)]
        public FloatAsset floatRef;
        [ShowIf("_checkType", checkType._float)]
        public float floatVal;

        //Bool
        [ShowIf("_checkType", checkType._bool)]
        public BoolAsset boolRef;
        [ShowIf("_checkType", checkType._bool)]
        public bool boolVal;

        //Items don't exist yet
        /*
                //Item
                [ShowIf("_checkType", checkType._item)]
                public Item itemVal;
                [ShowIf("_checkType", checkType._item)]
                public int itemAmount;
        */

        public bool isPageTrue()
        {

            switch (_checkType)
            {
                case checkType._bool:
                    if (boolVal == boolRef.Value)
                        return true;
                    else
                        return false;

                case checkType._int:
                    int i = intVal;
                    int j = intRef.Value;
                    switch (valueCheck)
                    {
                        case valueCheck.equal:
                            if (j == i)
                                return true;
                            else
                                return false;
                        case valueCheck.more:
                            if (j > i)
                                return true;
                            else
                                return false;
                        case valueCheck.less:
                            if (j < i)
                                return true;
                            else
                                return false;
                        case valueCheck.moreEqual:
                            if (j <= i)
                                return true;
                            else
                                return false;
                        case valueCheck.lessEqual:
                            if (j <= i)
                                return true;
                            else
                                return false;
                    }
                    break;

                case checkType._float:
                    float f = floatVal;
                    float m = floatRef.Value;
                    switch (valueCheck)
                    {
                        case valueCheck.equal:
                            if (m == f)
                                return true;
                            else
                                return false;
                        case valueCheck.more:
                            if (m > f)
                                return true;
                            else
                                return false;
                        case valueCheck.less:
                            if (m < f)
                                return true;
                            else
                                return false;
                        case valueCheck.moreEqual:
                            if (m <= f)
                                return true;
                            else
                                return false;
                        case valueCheck.lessEqual:
                            if (m <= f)
                                return true;
                            else
                                return false;
                    }
                    break;

                case checkType._item:
                    break;
                case checkType._quest:
                    break;
            }
            Debug.LogError("A non-existant check type was requested.");
            return false;
        }
    }

    public enum valueCheck
    {
        equal, more, less, moreEqual, lessEqual
    }
}