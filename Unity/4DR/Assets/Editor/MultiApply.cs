using UnityEngine;
using System.Collections;
using UnityEditor;
//using System;


public class MultiApply : EditorWindow
{
    public static MultiApply instance;

    [MenuItem("Edit/Object Active Change %g")]
    static void Init()
	{
        Object[] objList = GetSelectedObject(typeof(GameObject));

        for (int i = 0; i < objList.Length; ++i)
        {
            GameObject GO = objList[i] as GameObject;
            //PrefabUtility.ReplacePrefab(GO, PrefabUtility.GetPrefabParent(objList[i]), ReplacePrefabOptions.ConnectToPrefab);
			NGUITools.SetActive(GO, !GO.active);
            Debug.Log(GO.name + " : 적용 완료");
        }
	}


    // 선택된 Object의 갯수를 알아온다.
    public int GetSelectedObjectCount()
    {        
        Object[] selectObject = GetSelectedObject(typeof(GameObject));

        int count = selectObject.Length;

        return count;
    }


    // 선택된 오브젝트중에 원하는 타입의 오브젝트를 얻어온다.
    static public Object[] GetSelectedObject(System.Type type)
    {
        return Selection.GetFiltered(type, SelectionMode.DeepAssets);
    }


}
