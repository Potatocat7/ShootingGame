using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using System.Text;

public class WebViewOpneInRest : MonoBehaviour
{
    //private string url = "https://www.google.co.jp/";
    WebViewObject webViewObject;
    private string _htmlTextfile;

    //StreamWriter sw = new StreamWriter(ここはファイルのpath, true);

    // Start is called before the first frame update
    void Start()
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
    }
    public void OpenWebViewScene(string html)
    {
        //sw.Write("<table border=1>");

        webViewObject.Init((msg) =>
        {
#if UNITY_EDITOR_WIN
            if (msg.Equals("click"))
            {
                //GetComponent<GreeWebViewEditorWindow>().CallFromJS(msg);
                Debug.Log(msg);
            }

#endif
        });
#if UNITY_EDITOR_WIN
        // 書き込み
        //string path = Path.Combine(Application.persistentDataPath, "sample.html");
        string path = Application.dataPath + "/StreamingAssets/sample.html";
        bool isAppend = false; // 上書き or 追記
        _htmlTextfile = "<html><head><meta http-equiv=\"Content-Type\"content=\"text/html;charset=UTF-8\"><title>Ranking View!</title></head><body><h1>Ranking list</h1><hr/>";
        _htmlTextfile += html;
        _htmlTextfile += "<hr/><button type=\"submit\" onclick=\"Unity.call('click');\"><font size=\"5\">Close</font></button></body></html>";
        using (var fs = new StreamWriter(path, isAppend, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            fs.Write(_htmlTextfile);
        }
        webViewObject.LoadURL("file://" + Application.dataPath + "/StreamingAssets/sample.html");
#elif UNITY_ANDROID
        string path = Path.Combine(Application.persistentDataPath, "sample.html");
        //string path = "file:///android_asset/sample.html";
        bool isAppend = false; // 上書き or 追記
        _htmlTextfile = "<html><head><meta http-equiv=\"Content-Type\"content=\"text/html;charset=UTF-8\"><title>Ranking View!</title></head><body><h1>Ranking list</h1><hr/>";
        _htmlTextfile += html;
        _htmlTextfile += "<hr/><button type=\"submit\" onclick=\"Unity.call('click');\"><font size=\"5\">Close</font></button></body></html>";
        using (var fs = new StreamWriter(path, isAppend, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            fs.Write(_htmlTextfile);
        }
        webViewObject.LoadHTML(_htmlTextfile,path);
        //webViewObject.LoadURL("file:///android_asset/sample.html");
#endif
        // 中央に配置
        webViewObject.SetMargins(Screen.width / 10, Screen.height / 10, Screen.width / 10, Screen.height / 10);
        webViewObject.SetVisibility(true);
    }
}
