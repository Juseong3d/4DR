using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class BuildAssetBundle : EditorWindow {

	public string _buildName = "";

	public static BuildAssetBundle instance;

	public BuildTarget _buildTarget = BuildTarget.Android;

	Object[] selection;

	[MenuItem("Assets/Build AssetBundle")]
	static void Init()
	{
		if (instance != null)
		{
			instance.ShowUtility();
			return;
		}
		else
		{
			instance = (BuildAssetBundle)EditorWindow.GetWindow(typeof(BuildAssetBundle), false, "BuildAssetBundle");
			instance.ShowUtility();
		}
	}

	void OnGUI()
	{
		GUILayout.Space(5);
		_buildName = EditorGUILayout.TextField("BuildName : ", _buildName);
		GUILayout.Space(5);
		_buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("BuildTarget :", _buildTarget);
		

		if (GUILayout.Button("BuildAll"))
		{
			if (EditorUtility.DisplayDialog("Check", "선택한 오브젝트를 전체 빌드 하시겠습니까?", "빌드", "취소"))
			{
				EditorUtility.DisplayDialog("Check", BuildAll(), "확인");
			}			
		}
	}

	public string BuildAll(){

		selection = Selection.GetFiltered ( typeof ( Object ), SelectionMode.DeepAssets );

		string tmpString = string.Empty;

		if(_buildName.Length == 0) {
			tmpString = "에셋 번들의 이름을 입력하세요.";
			return tmpString;
		}

		if (_buildName.Length != 0) {

			BuildPipeline.BuildAssetBundle(
				Selection.activeObject,
				selection,
				"_BUNDLE/" + _buildTarget.ToString() + "/" + _buildName + ".unity3d",
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
				_buildTarget);

			Selection.objects = selection;
		}
		
		tmpString = "완료.";

		return tmpString;
	}


}
