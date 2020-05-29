using UnityEngine;

internal class UIDEFINE {

	public const string PATH_POPUP_BOX_BACK = "Common/_Default_GUI/Popup/_pfb_POPUP_BOX_BACK";
    public const string PATH_POPUP_BOX_BACK_9000 = "Common/_Default_GUI/Popup/_pfb_POPUP_BOX_BACK_9000";
    public const string PATH_POPUP_BOX = "Common/_Default_GUI/Popup/_pfb_POPUP_BOX";

    //font
    public const string FONT_GODAB = "Common/_FONT/fnt_GODAB";

	//prefab
	internal static readonly string PATH_INTRO = "Common/_Default_GUI/pfb_Intro";


    public const string TEST_EFFECT = "Common/_Default_Effect/pfb_Effect_Touch";

    public const string PATH_VIDEO_MANAGER = "Common/_Default_GUI/pfb_VideoManager";
    public const string PATH_VIDEO_CTL = "Common/_Default_GUI/pfb_FDPlayerCTL";


	static public UIAtlas getProjectAtlas(PROJECT_TYPE type, string str){
    
        string tmpPath = "Atlas/";
        string tmpAtlasName = str;

        UIAtlas atlas;

        switch(type){
        
			default :
                tmpPath += tmpAtlasName;
				break;

        }

        //UIAtlas atals = Resources.Load(path, typeof(UIAtlas)) as UIAtlas;
        atlas = Resources.Load(tmpPath, typeof(UIAtlas)) as UIAtlas;

        if(atlas == null) {
            atlas = Resources.Load(("Atlas/" + str), typeof(UIAtlas)) as UIAtlas;
        }

        return atlas;

    }


    static public Font setProjectFont(PROJECT_TYPE type) {

        UIFont newFont;
        
        switch(type) {
            default :
                newFont = Resources.Load((UIDEFINE.FONT_GODAB), typeof(UIFont)) as UIFont;
            break;
        }

        return newFont.dynamicFont;

    }

}