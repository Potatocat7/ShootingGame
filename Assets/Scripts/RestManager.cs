using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using MiniJSON;
using UnityEngine.UI;

[DataContract]
public class Answer
{
    // JSONのフィールド名とプロパティ名は合わせなければいけません。
    [DataMember]
    public string name;
    [DataMember]
    public int value;
}
public class RestManager : MonoBehaviour
{
    //#if UNITY_IPHONE || UNITY_ANDROID
    //#else
    //#endif
    [SerializeField]
    private Text _logText;
    [SerializeField]
    private Text _rankingText;
    [SerializeField]
    private WebViewOpneInRest _webViewObject;

    //    //private string _rankingListHtmlData;
    //    /*enum urlCommandTypeKind
    //      {
    //          delete_all = 0,
    //          write,
    //          count,
    //          read
    //      }*/
    //    static async Task WriteSample(HttpClient client, string score, Text log)
    //    {
    //        var url_write = "http://172.20.129.234:8081/v1/score";
    //        //write_sample
    //        var json_wr = "{\"id\": 1234,\"value\": " + score + "}";
    //        var content_wr = new StringContent(json_wr, Encoding.UTF8, "application/json");
    //        var httpResponse_wr = await client.PostAsync(url_write, content_wr);
    //        //json_wr = "{\"id\": 1235,\"value\": 89}";
    //        //content_wr = new StringContent(json_wr, Encoding.UTF8, "application/json");
    //        //httpResponse_wr = await client.PostAsync(url_write, content_wr);
    //        //json_wr = "{\"id\": 1236,\"value\": 79}";
    //        //content_wr = new StringContent(json_wr, Encoding.UTF8, "application/json");
    //        //httpResponse_wr = await client.PostAsync(url_write, content_wr);
    //#if UNITY_IPHONE || UNITY_ANDROID
    //        log.text += "add:" + score + "\n";
    //#else
    //#endif
    //    }
    //    public static async Task ReadSample(HttpClient client,Text rank,Text log)
    //    {
    //        //var answer = new Answer();
    //        var url_read = "http://172.20.129.234:8081/v1/score?begin=0&end=9";
    //        //read_sample
    //        var httpResponse_rd = await client.GetAsync(url_read);
    //        var responseContent_rd = await httpResponse_rd.Content.ReadAsStringAsync();
    //        //JSONファイル全体をまとめて文字列化
    //        string strResponse = Json.Serialize(responseContent_rd);
    //        //dic化最上層
    //        var response = Json.Deserialize(responseContent_rd) as Dictionary<string, object>;
    //        IList values = (IList)response["values"];

    //#if UNITY_IPHONE || UNITY_ANDROID
    //        log.text += strResponse + "\n";
    //#else
    //        Debug.Log("obj:" + strResponse);
    //        Debug.Log((string)response["name"]);
    //        for (int i = 0; i < values.Count; i++)
    //        {
    //            var value = values[i] as Dictionary<string, object>;
    //            Debug.Log((string)value["id"]);
    //            Debug.Log((int)(long)value["user"]); 
    //            Debug.Log((string)value["time"]);
    //            Debug.Log((long)value["value"]);
    //        }
    //#endif
    //        List<int> ranking = new List<int>();
    //        for (int i = 0; i < values.Count; i++)
    //        {
    //            //配列の階層があるたびにIListに入れてループで1個1個設定しなおす必要がある
    //            var value = values[i] as Dictionary<string, object>;
    //            ranking.Add((int)(long)value["value"]);
    //        }
    //        // rankingをラムダ式でソート //ソートされたものが来るなら不要
    //        //ranking.Sort((a, b) => b - a);
    //        if (ranking.Count > 10)//ランキングを10固定　制限なしならelse側だけで良い
    //        {
    //            for (int i = 0; i <10; i++)
    //            {
    //                rank.text += "<p align=\"center\">" + (i+1).ToString() + "位：" + ranking[i].ToString() + "</p>";
    //                //rank += ranking[i].ToString() + "\n";
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < ranking.Count; i++)
    //            {
    //                rank.text += "<p align=\"center\">" + (i + 1).ToString() + "位：" + ranking[i].ToString() + "</p>";
    //                //rank.text += ranking[i].ToString() + "\n";
    //                //rank += ranking[i].ToString() + "\n";
    //            }
    //            for (int i = ranking.Count; i < 10; i++)//たぶんいらないからあとで削除
    //            {
    //                rank.text += "<p align=\"center\">" + (i + 1).ToString() + "位：" + "---" + "</p>";
    //                //rank.text += "---" + "\n";
    //                //rank += "---" + "\n";
    //            }
    //        }
    //    }

    //    public static async Task CountSample(HttpClient client, Text log)
    //    {
    //        var answer = new Answer();
    //        var url_count = "http://172.20.129.234:8081/v1/score_count";
    //        //count_sample
    //        var httpResponse_cnt = await client.GetAsync(url_count);//, content_cnt);
    //        var responseContent_cnt = await httpResponse_cnt.Content.ReadAsStringAsync();
    //        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(responseContent_cnt)))
    //        {
    //            var ser = new DataContractJsonSerializer(answer.GetType());
    //            answer = ser.ReadObject(ms) as Answer;
    //        }
    //#if UNITY_IPHONE || UNITY_ANDROID
    //        log.text += "name:" + answer.name + "\n";
    //        log.text += "value:" + answer.value + "\n";
    //#else
    //        Debug.Log("name:" + answer.name);
    //        Debug.Log("value:" + answer.value);
    //#endif
    //    }
    //    public static async Task DeleteSample(HttpClient client, Text log)
    //    {
    //        var url_delete_all = "http://172.20.129.234:8081/v1/admin/delete_all";
    //        //delet_all_sample
    //        var json_del = "{\"id\": 1234}";
    //        var content_del = new StringContent(json_del, Encoding.UTF8, "application/json");
    //        var httpResponse_del = await client.PostAsync(url_delete_all, content_del);
    //#if UNITY_IPHONE || UNITY_ANDROID
    //        log.text += "Delete\n";
    //#else
    //#endif
    //    }
    //    static async Task<string> SendDataServer(string score,Text ranking, WebViewOpneInRest webview, Text log)
    //    {
    //        //string url_delete_all = "http://localhost:8081/v1/admin/delete_all";
    //        //string url_write = "http://localhost:8081/v1/score";
    //        //string url_count = "http://localhost:8081/v1/score_count";
    //        //string url_read = "http://localhost:8081/v1/score";
    //        using (var client = new HttpClient())
    //        {
    //            //await DeleteSample(client, log);
    //            await WriteSample(client,score, log);
    //            await CountSample(client, log);
    //            await ReadSample(client, ranking, log);//今はここでソートしているため順位のリストを引数に入れる
    //            //await DeleteSample(client, log);
    //            //list = await ReadSample(client,test,log);
    //            //Debug.Log("");
    //            //ここにwebview呼び出し ※↑でhtmlファイルの変数を用意するためここもawaitが必要？
    //            //webview.OpenWebViewScene();
    //            return await Task.FromResult(ranking.text.ToString());//もってきたランキング用のstringをもっていく
    //        }
    //    }
    //    public async void SendScoreByREST(int score)//持っていきたい変数は引数にする
    //    {
    //        var input = score.ToString();
    //        string test = await SendDataServer(input, _rankingText, _webViewObject, _logText);//ランキングを入れるリストをもっていく(stringで繋げた状態にしておく？)
    //        _webViewObject.OpenWebViewScene(test);

    //    }
}
