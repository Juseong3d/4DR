#define _PATCH

using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class CSVReader
{
	static public string[] ReadFile(string path, bool loadAssetBundle = true)
	{
		try
		{
			TextAsset RF = null;
            
			//if (loadAssetBundle)
			//	RF = (TextAsset)_Resources.Load(path, typeof(TextAsset));
			//else

#if _ASSET_BUNDLE_
			RF = Appmain.appimg.bundleMgr.bundle[DEFINE.ASSET_TABLE_IDX].Load(path, typeof(TextAsset)) as TextAsset;
#else
			RF = (TextAsset)Resources.Load(path, typeof(TextAsset));
#endif

			if (RF == null)
			{
				return null;
			}
			
			string[] bufferArray = RF.text.Split("\n"[0]);

			string[] baseHeader = bufferArray[0].Split(","[0]);			
			int line = Convert.ToInt32(baseHeader[0]);			
			string[] allData = new string[line + 1];

			for (int i = 0; i < line+1; ++i)
			{
				allData[i] = bufferArray[i];//.Replace("|", "\u000A\u000D"); 
			}
			Debug.Log("readFile :: " + path);
			return allData;
		}
		catch(System.Exception ex)
		{
			Debug.Log("Exception  : " + ex);
			return null;
		}
	}


	static public string[] ReadFileFromString(string RF, bool loadAssetBundle = true)
	{
		try
		{
			if (RF == null)
			{
				return null;
			}
			
			string[] bufferArray = RF.Split("\n"[0]);

			string[] baseHeader = bufferArray[0].Split(","[0]);			
			//int line = Convert.ToInt32(baseHeader[0].Trim());

			//Debug.Log("baseHeader[0] ::: " + baseHeader[0]);
			string what = baseHeader[0].Trim();

			int line = int.Parse(what);
			string[] allData = new string[line + 1];

			for (int i = 0; i < line+1; ++i)
			{
				allData[i] = bufferArray[i];//.Replace("|", "\u000A\u000D"); 
			}
			//Debug.Log("readFile :: " + RF);
			return allData;
		}
		catch(System.Exception ex)
		{
			Debug.Log("Exception  : " + ex);
			return null;
		}
	}


	static public TextAsset GetText2ReadFile(string path)
	{
		try
		{
//#if _PATCH
//			TextAsset RF = (TextAsset)_Resources.Load(path, typeof(TextAsset));
//#else
//			TextAsset RF = (TextAsset)Resources.Load(path, typeof(TextAsset));
//#endif			
			TextAsset RF = (TextAsset)Resources.Load(path, typeof(TextAsset));

			return RF;
		}
		catch (System.Exception ex)
		{
			Debug.Log("Exception  : " + ex);
			return null;
		}
	}


	static public string[] ReadFileOut(string path)
	{
		Debug.Log("path = " + path);
		try
		{
			StreamReader sr = new StreamReader(path);
			string lineCount = sr.ReadLine();
			string[] tmp = lineCount.Split(","[0]);
			int line = Convert.ToInt32(tmp[0]);
			string[] allData = new string[line + 1];
			allData[0] = lineCount;
			for(int i=0; i < line; ++i)
			{
				allData[i+1] = sr.ReadLine();
			}

			Debug.Log("now ReadFileOut = " + path);
			return allData;
		}
		catch (System.Exception ex)
		{
			Debug.Log("Exception  : " + ex);
			return null;
		}
	}

	// 안쓰는거
	//static public string getLineData4One(int needLineIndex, string[] src)
	//{
	//    if(src.Length <= needLineIndex)
	//    {
	//        Debug.Log("src Length : " + src.Length + "  needLineIndex	: "+needLineIndex);
	//        return null;
	//    }
	//    return src[needLineIndex];
	//}


	//static public string[] getTableData(string src)
	//{
	//    string[] cutSrc = null;

	//    cutSrc = src.Split(","[0]);		//    return cutSrc;
	//}
}
