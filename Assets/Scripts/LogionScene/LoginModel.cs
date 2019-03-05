using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.IO;


public class LoginModel : MonoSingleton<LoginModel> 
{
    Dictionary<string, string> userDic = new Dictionary<string, string>();
    Dictionary<string, BasePlayer> playerDic = new Dictionary<string, BasePlayer>();
    public Dictionary<int, string> severDic = new Dictionary<int, string>();
    
    string directoryPath = null;   //文件夹的路径
    string userFileName  = null;   //文件名字
    string severFileName = null;   //文件名字

    public override void OnInitSingleton()
    {
        directoryPath = Application.dataPath + @"/Resources/Configs";//文件夹的路径
        userFileName = "User.xml";   //文件名字
        severFileName = "Sever.xml";


        loadUserXml();

        loadSeverXml();
    }

    #region  加载服务器信息
    void loadSeverXml()
    {
        //确认路径
        string path = directoryPath + "//" + severFileName;

        //解析 Xml，添加字典
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNodeList SeverList = xmlDoc.FirstChild.SelectNodes("sever");

        foreach (XmlElement severElement in SeverList)
        {
            int SeverID = int.Parse(severElement.GetAttribute("SeverID"));
            string SeverName = severElement.GetAttribute("SeverName");
            severDic[SeverID] = SeverName;
        }

    }
    #endregion

    #region  加载帐号信息
    //加载帐号密码（xml）
    void loadUserXml()
    {
        //确认路径
        string path = directoryPath + "//" + userFileName;

        //先确认文件是否存在
        if (File.Exists(path))
        {
            //解析 Xml，添加字典
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList userList = xmlDoc.FirstChild.SelectNodes("user");

            foreach (XmlElement userElement in userList)
            {
                if (userElement.GetAttribute("userName") != "")
                {
                    string username = userElement.GetAttribute("userName");
                    string password = userElement.GetAttribute("password");
                    userDic[username] = password;

                    XmlNodeList playerList = userElement.SelectNodes("player");
                    foreach (XmlElement playerElement in playerList)
                    {
                        if (playerElement.GetAttribute("PlayerID") != "")
                        {
                            BasePlayer go;
                            go.PlayerID= playerElement.GetAttribute("PlayerID");
                            go.UserID= playerElement.GetAttribute("UserID");
                            go.ServerNum = int.Parse(playerElement.GetAttribute("ServerNum"));

                            go.Name= playerElement.GetAttribute("Name");
                            go.Sex=(SexType) int.Parse(playerElement.GetAttribute("Sex"));
                            go.HeadImageId = int.Parse(playerElement.GetAttribute("HeadImageId"));

                            playerDic[go.PlayerID] = go;
                        }

                    }
                }
            }

        }
        else  //文件不存在，手动创建
        {
            createUserXml(path);
        }
    }

    
    //创建 Xml 文件 
    void createUserXml(string path)
    {
        //1.文件夹是否存在
        if (!Directory.Exists(directoryPath))
        {
            //文件夹不存在，手动创建
            Directory.CreateDirectory(directoryPath);
        }

        //2.创建 xml文件
        XmlDocument xmlDoc = new XmlDocument();

        XmlNode root = xmlDoc.CreateElement("root");
        xmlDoc.AppendChild(root);

        XmlElement element = xmlDoc.CreateElement("user");
        element.SetAttribute("userName", "");
        element.SetAttribute("password", "");

        root.AppendChild(element);

        xmlDoc.Save(path);
    }

    #endregion



    //判断用户是否存在
    public bool IsHaveUserName(string _userName)
    {
        return userDic.ContainsKey(_userName);
    }

    //登录是否成功
    public bool LoginSuccess(string username, string password)
    {
        if (userDic.ContainsKey(username))
        {
            return userDic[username] == password;
        }

        return false;
    }

    //注册是否成功
    public bool RegisterSuccess(string username, string password)
    {
        string path = directoryPath + "/" + userFileName;

        if (File.Exists(path))
        {
            //当前账号已经被注册过了
            if (userDic.ContainsKey(username))
            {
                return false;
            }
        }
        else
        {
            createUserXml(path);
        }

        //注册  字典 + Xml
        AddElementToXml(path, username, password);

        return true;
    }

    void AddElementToXml(string path, string username, string password)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNode root = xmlDoc.SelectSingleNode("root");

        XmlElement element = xmlDoc.CreateElement("user");
        element.SetAttribute("userName", username);
        element.SetAttribute("password", password);
        root.AppendChild(element);

        xmlDoc.Save(path);

        //---------------- 添加字典
        userDic[username] = password;
    }

}
