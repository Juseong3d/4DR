using UnityEngine;

internal class UIDEFINE {

	public const string PATH_POPUP_BOX_BACK = "Common/_Default_GUI/Popup/_pfb_POPUP_BOX_BACK";
    public const string PATH_POPUP_BOX_BACK_9000 = "Common/_Default_GUI/Popup/_pfb_POPUP_BOX_BACK_9000";
    public const string PATH_POPUP_BOX = "Common/_Default_GUI/Popup/_pfb_POPUP_BOX";

    //font
    public const string FONT_GODAB = "Common/_FONT/fnt_GODAB";


#if _TAE_
    public const string PATH_INTRO = "Common/_Default_GUI/TAE/pfb_Intro";
    public const string PATH_TITLE = "Common/_Default_GUI/TAE/pfb_TITLE";
    public const string PATH_MAIN_MENU = "Common/_Default_GUI/TAE/pfb_MAIN_MENU";
    public const string PATH_VIDEO_ITEM_MINI = "Common/_Default_GUI/TAE/pfb_VideoManager_Mini2";

    public const string PATH_PLAYER_INFO = "Common/_Default_GUI/TAE/pfb_player_vs";
#else	
	public const string PATH_INTRO = "Common/_Default_GUI/pfb_Intro";
    public const string PATH_TITLE = "Common/_Default_GUI/pfb_TITLE";
    public const string PATH_MAIN_MENU = "Common/_Default_GUI/pfb_MAIN_MENU";
    public const string PATH_VIDEO_ITEM_MINI = "Common/_Default_GUI/pfb_VideoManager_Mini2";
#endif

    public const string TEST_EFFECT = "Common/_Default_Effect/pfb_Effect_Touch";

    public const string PATH_VIDEO_MANAGER = "Common/_Default_GUI/pfb_VideoManager2";
    public const string PATH_VIDEO_CTL = "Common/_Default_GUI/pfb_FDPlayerCTL";    
    
	public const string PATH_CAMERA_SCRIPT_ALL = "Common/_Default_Table/CameraScript";

    public const string PATH_CAMERA_SCRIPT_ITEM = "Common/_Default_GUI/pfb_ITEM_Camera_Script";
	

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