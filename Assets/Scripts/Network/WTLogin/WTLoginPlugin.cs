
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR

public struct QZoneResponse
{
    public int ret;
    public string msg;
    public string openid;
    public string token;
    public int expire;
    public string keysignature;

    public void Reset()
    {
        ret = 0;
        msg = null;
        openid = null;
        token = null;
        expire = 0;
        keysignature = null;
    }

    public override string ToString()
    {
        return "ret:" + ret + "\nmsg:" + msg + "\nopenid:" + openid + "\ntoken:" + token + "\nexpire:" + expire + "\nkeysignature:" + keysignature;
    }

}

public class WTLoginPlugin{

    public const int WLOGIN_SUCCESS = 0;

    /*************************错误码定义**************/
    /* 需要检验验证码 */
    public enum WTLoginErrCode
    {
        WLOGIN_CTRL_RES_NEED_CHECK_PIC = 100,
        WLOGIN_CTRL_TCP_CONN_FAILED = 101,
        WLOGIN_CTRL_SEND_DATA_ERR = 102,
        WLOGIN_CTRL_GET_RESPONSE_ERR = 103,
        WLOGIN_CTRL_PARSE_RESPONSE_ERR = 104,
        WLOGIN_CTRL_ERR_REPLYCODE = 105,
        WLOGIN_CTRL_ERR_UNKNOW = 106,
        WLOGIN_CTRL_RES_NEED_CHALLENGE = 110,
        WLOGIN_CTRL_RES_NEED_CHECK_MESSAGE = 111,

        WLOGIN_INPUT_NULL_POINTER = -3,
        WLOGIN_ERR_INPUT_PARAM = -4,
        WLOGIN_MEM_NOT_ENOUGH = -5,

        WLOGIN_REQ_GEN_TLV_ERR = -50,
        WLOGIN_REQ_ADD_RANDKEY_ERR = -51,

        WLOGIN_RES_ERR_PACKET = -100,
        WLOGIN_RES_UNEXPECTED_DATA = -101,
        WLOGIN_RES_CHECK_SEQ_ERR = -102,
        WLOGIN_RES_CAL_PKG_LEN_ERR = -103,
        WLOGIN_RES_TLV_ERR = -104,
    }


    public const int kSkeyLength = 16;      //this is the length for skey used by wtlogin

    // dll 初始化
    [DllImport("WTLogin")]
    public static extern int login_with_password(uint account, string password, byte[] sKey);



    public static string GetQZoneServer(int appid,uint uin,string sKey)
    {
        return string.Format("http://openmobile.qq.com/oauth2.0/m_token_reapply?oauth_consumer_key={0}&uin={1}&skey={2}", appid, uin, sKey);
    }

    public static void ParseQZoneResponse(ref QZoneResponse response, string data)
    {
        XmlDocument doc = new XmlDocument();


        // an example of successful response
        //<data>
        //<ret>0</ret>
        //<msg>ok</msg>
        //<openid>C4BAE34AE71E8F4BDEE7D86698511615</openid>
        //<token>BC640E3A43E6C547039BC116041D1419</token>
        //<expire>7776000</expire>
        //<keysignature>cbbbe0b768730e2593d204505cd67cdc</keysignature>
        //</data>


        doc.LoadXml(data);

        XmlNodeList nodeList = doc.SelectSingleNode("/data").ChildNodes;
        foreach (XmlNode xn in nodeList)    //遍历所有子节点
        {
            XmlElement xe = (XmlElement)xn;
            switch (xe.Name)
            {
                case "ret":
                    int.TryParse(xe.InnerText,out response.ret);
                    break;
                case "msg":
                    response.msg = xe.InnerText;
                    break;
                case "openid":
                    response.openid = xe.InnerText;
                    break;
                case "token":
                    response.token = xe.InnerText;
                    break;
                case "expire":
                    int.TryParse(xe.InnerText, out response.expire);
                    break;
                case "keysignature":
                    response.keysignature = xe.InnerText;
                    break;
            }
        }
    }

}

#endif
