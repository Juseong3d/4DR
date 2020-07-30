using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

        public enum WMessages : int
        {

            WM_LBUTTONDOWN = 0x201, //Left mousebutton down

            WM_LBUTTONUP = 0x202,  //Left mousebutton up

            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick

            WM_RBUTTONDOWN = 0x204, //Right mousebutton down

            WM_RBUTTONUP = 0x205,   //Right mousebutton up

            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick

            WM_KEYDOWN = 0x100,  //Key down

            WM_KEYUP = 0x101,   //Key up

        }

        public enum VKeys : int

        {

            //Keys used for gaming.

            Heal_Complete = VK_3,

            Heal_Normal = VK_4,

            Heal_Team = VK_5,

            Assist = VK_2,

            //-----END-----

            VK_LBUTTON = 0x01,   //Left mouse button 

            VK_RBUTTON = 0x02,   //Right mouse button 

            VK_CANCEL = 0x03,   //Control-break processing 

            VK_MBUTTON = 0x04,   //Middle mouse button (three-button mouse) 

            VK_BACK = 0x08,   //BACKSPACE key 

            VK_TAB = 0x09,   //TAB key 

            VK_CLEAR = 0x0C,   //CLEAR key 

            VK_RETURN = 0x0D,   //ENTER key 

            VK_SHIFT = 0x10,   //SHIFT key 

            VK_CONTROL = 0x11,   //CTRL key 

            VK_MENU = 0x12,   //ALT key 

            VK_PAUSE = 0x13,   //PAUSE key 

            VK_CAPITAL = 0x14,   //CAPS LOCK key 

            VK_ESCAPE = 0x1B,   //ESC key 

            VK_SPACE = 0x20,   //SPACEBAR 

            VK_PRIOR = 0x21,   //PAGE UP key 

            VK_NEXT = 0x22,   //PAGE DOWN key 

            VK_END = 0x23,   //END key 

            VK_HOME = 0x24,   //HOME key 

            VK_LEFT = 0x25,   //LEFT ARROW key 

            VK_UP = 0x26,   //UP ARROW key 

            VK_RIGHT = 0x27,   //RIGHT ARROW key 

            VK_DOWN = 0x28,   //DOWN ARROW key 

            VK_SELECT = 0x29,   //SELECT key 

            VK_PRINT = 0x2A,   //PRINT key

            VK_EXECUTE = 0x2B,   //EXECUTE key 

            VK_SNAPSHOT = 0x2C,   //PRINT SCREEN key 

            VK_INSERT = 0x2D,   //INS key 

            VK_DELETE = 0x2E,   //DEL key 

            VK_HELP = 0x2F,   //HELP key

            VK_OEMCOMMA = 0xBC, //OEMComma

            VK_0 = 0x30,   //0 key 

            VK_1 = 0x31,   //1 key 

            VK_2 = 0x32,   //2 key 

            VK_3 = 0x33,   //3 key 

            VK_4 = 0x34,   //4 key 

            VK_5 = 0x35,   //5 key 

            VK_6 = 0x36,    //6 key 

            VK_7 = 0x37,    //7 key 

            VK_8 = 0x38,   //8 key 

            VK_9 = 0x39,    //9 key 

            VK_A = 0x41,   //A key 

            VK_B = 0x42,   //B key 

            VK_C = 0x43,   //C key 

            VK_D = 0x44,   //D key 

            VK_E = 0x45,   //E key 

            VK_F = 0x46,   //F key 

            VK_G = 0x47,   //G key 

            VK_H = 0x48,   //H key 

            VK_I = 0x49,    //I key 

            VK_J = 0x4A,   //J key 

            VK_K = 0x4B,   //K key 

            VK_L = 0x4C,   //L key 

            VK_M = 0x4D,   //M key 

            VK_N = 0x4E,    //N key 

            VK_O = 0x4F,   //O key 

            VK_P = 0x50,    //P key 

            VK_Q = 0x51,   //Q key 

            VK_R = 0x52,   //R key 

            VK_S = 0x53,   //S key 

            VK_T = 0x54,   //T key 

            VK_U = 0x55,   //U key 

            VK_V = 0x56,   //V key 

            VK_W = 0x57,   //W key 

            VK_X = 0x58,   //X key 

            VK_Y = 0x59,   //Y key 

            VK_Z = 0x5A,    //Z key

            VK_NUMPAD0 = 0x60,   //Numeric keypad 0 key 

            VK_NUMPAD1 = 0x61,   //Numeric keypad 1 key 

            VK_NUMPAD2 = 0x62,   //Numeric keypad 2 key 

            VK_NUMPAD3 = 0x63,   //Numeric keypad 3 key 

            VK_NUMPAD4 = 0x64,   //Numeric keypad 4 key 

            VK_NUMPAD5 = 0x65,   //Numeric keypad 5 key 

            VK_NUMPAD6 = 0x66,   //Numeric keypad 6 key 

            VK_NUMPAD7 = 0x67,   //Numeric keypad 7 key 

            VK_NUMPAD8 = 0x68,   //Numeric keypad 8 key 

            VK_NUMPAD9 = 0x69,   //Numeric keypad 9 key 

            VK_SEPARATOR = 0x6C,   //Separator key 

            VK_SUBTRACT = 0x6D,   //Subtract key 

            VK_DECIMAL = 0x6E,   //Decimal key 

            VK_DIVIDE = 0x6F,   //Divide key

            VK_F1 = 0x70,   //F1 key 

            VK_F2 = 0x71,   //F2 key 

            VK_F3 = 0x72,   //F3 key 

            VK_F4 = 0x73,   //F4 key 

            VK_F5 = 0x74,   //F5 key 

            VK_F6 = 0x75,   //F6 key 

            VK_F7 = 0x76,   //F7 key 

            VK_F8 = 0x77,   //F8 key 

            VK_F9 = 0x78,   //F9 key 

            VK_F10 = 0x79,   //F10 key 

            VK_F11 = 0x7A,   //F11 key 

            VK_F12 = 0x7B,   //F12 key

            VK_SCROLL = 0x91,   //SCROLL LOCK key 

            VK_LSHIFT = 0xA0,   //Left SHIFT key

            VK_RSHIFT = 0xA1,   //Right SHIFT key

            VK_LCONTROL = 0xA2,   //Left CONTROL key

            VK_RCONTROL = 0xA3,    //Right CONTROL key

            VK_LMENU = 0xA4,      //Left MENU key

            VK_RMENU = 0xA5,   //Right MENU key

            VK_PLAY = 0xFA,   //Play key

            VK_ZOOM = 0xFB, //Zoom key 

        }





public class Appmain : MonoBehaviour
{

    [DllImport("user32.dll")]
	public static extern int FindWindow(string lpClassName, string lpWindowName);
	//FindWindow (최상위 창 핸들값 가져오는 API)

	[DllImport("user32.dll")]
	public static extern int FindWindowEx(int hWnd1, int hWnd2, string lpsz1, string lpsz2);
	//FindWindowEX (인자로 받아온 핸들의 자식의 핸들값 가져오는 API)

	[DllImport("user32.dll")]
	public static extern int SendMessage(int hwnd, int wMsg, int wParam, string lParam);
	//SendMessage

	[DllImport("user32.dll")]
	public static extern uint PostMessage(int hwnd, int wMsg, int wParam, int lParam);
	//PostMessage

	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(int hWnd);

	[DllImport("user32.dll")]
	private static extern bool ShowWindowAsync(int hWnd, int nCmdShow);

    // Messages
    const int WM_KEYDOWN = 0x100;
    const int WM_KEYUP = 0x101;
    const int WM_CHAR = 0x105;
    const int WM_SYSKEYDOWN = 0x104;
    const int WM_SYSKEYUP = 0x105;
    const int WM_CLOSE = 0x0010;



	public Text _inputText;
    public Text _inputHowMany;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static void SendKatalk(string title, string msg)
	{
		int hd01 = FindWindow(title, null);			

		Debug.Log(hd01);

		int hd03 = FindWindowEx(hd01, 0, "RichEdit50W", "");
		//int hd04 = FindWindowEx(0, hd03, "RichEdit50W", "");

		if(hd03 == 0) Debug.Log("cant find RichEdit50W");
		SetForegroundWindow(hd01);

        byte[] _bMsg = Encoding.UTF8.GetBytes(msg);

        msg = Encoding.UTF8.GetString(_bMsg);
        //msg = Encoding.GetEncoding().GetString(msg);
        Debug.Log("msg :: " + msg);
		SendMessage(hd03, 0x000c, 0, msg);
		PostMessage(hd03, WM_KEYDOWN, 0xD, 0x1C001);

        PostMessage(hd01, WM_KEYDOWN, (int)VKeys.VK_ESCAPE, 0x1C001);

	}


	public void OnClickButton4Send() {

		Debug.Log("_inputText : " + _inputText.text);
		//byte[] _bytes = Encoding.UTF8.GetBytes(_inputText.text);
		//string tmp = Encoding.Unicode.GetString(_bytes);

		SendKatalk("#32770", _inputText.text);

	}


	public void OnClickButton4FindKK() {

        int _cnt;
        bool result = int.TryParse(_inputHowMany.text, out _cnt);

        if(result == false) {
            _cnt = 1;
        }

		Debug.Log("OnClickButton4FindKK()");
		int hd01 = FindWindow("EVA_Window_Dblclk", null);

		Debug.Log("hd01 == " + hd01);
		SetForegroundWindow(hd01);        

        int hDest = hd01;

        if (hDest != 0) { 
            hDest = FindWindowEx(hDest, 0, "EVA_ChildWindow", null); // 메인의 1번 자식   OnlineMainView

            if (hDest != 0) { 
                //int hDestEdit = FindWindowEx(hDest, 0, "EVA_Window", null); // 메인의 1번 자식의 1번 자식 ContactListView   
                int ContactListView = FindWindowEx(hDest, 0, "EVA_Window", null); // 메인의 1번 자식의 1번 자식 ContactListView //이건 친구 리스트
                //ContactListView = FindWindowEx(hDest, ContactListView, "EVA_Window", null); //한번 더 하면 채팅방 리스트

                int ContactListCtrl = FindWindowEx(ContactListView, 0, "EVA_VH_ListControl_Dblclk", null);  //SearhListCtrl
                ContactListCtrl = FindWindowEx(ContactListView, ContactListCtrl, "EVA_VH_ListControl_Dblclk", null);    //두번째인 ContactListCtrl

                
                //PostMessage(ContactListCtrl, WM_KEYDOWN, (int)VKeys.VK_RETURN, 0x1C001);    //앤터키로 방진입은 됨

                for(int i = 0; i<_cnt; i++) {
                    PostMessage(ContactListCtrl, WM_KEYDOWN, (int)VKeys.VK_DOWN, 0x1C001);
                    PostMessage(ContactListCtrl, WM_KEYDOWN, (int)VKeys.VK_RETURN, 0x1C001);    //앤터키로 방진입은 됨
                    Thread.Sleep(100);
                    OnClickButton4Send();
                    
                }
                

                //if (hDest != 0) { 
                //    hDestEdit = FindWindowEx(hDest, hDestEdit, "EVA_Window", null); //메인의 1번자식의 2번자식 

                //    PostMessage(hDestEdit, 0x0100, 0xD, 0x1C001); 

                    //if (hDestEdit != 0) { 
                    //    hDestEdit = FindWindowEx(hDestEdit, 0, "Edit", null); //메인의 1번자식의 2번자식의 1번자식 
                    //    if (hDestEdit != 0) { 
                    //        SendMessage(hDestEdit, 0x000c, 0, "내이름"); 
                    //        Thread.Sleep(1000); 
                    //        PostMessage(hDestEdit, 0x0100, 0xD, 0x1C001); 
                    //    } 
                    //} 
            } 
        } 
        

        ////for(int i = 0; i<10; i++) 
        //{
        //    StartCoroutine(PressKey(hd01));
        //}

	}


    public void OnClickButton4TestKey() {


        Debug.Log("OnClickButton4TestKey()");

        int hd01 = FindWindow("EVA_Window_Dblclk", null);

        Debug.Log("hd01 :: " + hd01);
        SetForegroundWindow(hd01);        
        //yield return new WaitForEndOfFrame();

        
        //for(int i = 0; i<10; i++) 
        {
            //Debug.Log("i = " + i);
            PostMessage(hd01, WM_KEYDOWN, (int)VKeys.VK_RETURN, 0x1C001);
        }

    }

    IEnumerator PressKey(int hd01) {

        yield return new WaitForSeconds(1.0f); //WaitForEndOfFrame();

        SetForegroundWindow(hd01);

        Debug.Log("calll : " + hd01);
        PostMessage(hd01, WM_KEYDOWN, (int)VKeys.VK_RETURN, 0x1C001);

    }


	public void OnClickButtonFindDbox() {

		Debug.Log("OnClickButtonFindDbox()");

		int hd01 = FindWindow("#32770", null);

		Debug.Log("hd01 == " + hd01);
		SetForegroundWindow(hd01);
        //SendMessage(hd01, (int)VKeys.VK_ESCAPE, 0, "");
        PostMessage(hd01, WM_KEYDOWN, (int)VKeys.VK_ESCAPE, 0x1C001);   //창닫기


	}

}
