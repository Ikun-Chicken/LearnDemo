using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> 
{
    //ui名字对应ui所处的路径
    private Dictionary<string, string> pathDic;
    public UIManager()
    {
        InitDic();
    }
    private void InitDic()
    {
        pathDic = new Dictionary<string, string>()
        {
            {UIContants.Main,"Main" },
            {UIContants.Missions, "Missions"},
            {UIContants.Setting,"Setting" },
            {UIContants.Shop,"Shop" },
            {UIContants.Character,"Character" },
        };
    }

    private Transform root;
    //预制件资源的缓存，方便读取资源
    private Dictionary<string, GameObject> prefabDic=new Dictionary<string, GameObject>();
    //已经打开的界面的实例的缓存，方便销毁
    public Dictionary<string, BasePanel> panelDic=new Dictionary<string, BasePanel>();
    public Transform Root
    {
        get
        {
            if (root == null)
                root = GameObject.Find("RootCanvas").transform;
            return root;
        }
    }
    /// <summary>
    /// 传入要打开的界面，返回这个界面
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        if(panelDic.TryGetValue(name, out panel))
        {
            Debug.Log("已经打开"); return null;
		}
        string path = "";
        //根据这个名字去字典找到对应路径
        if(!pathDic.TryGetValue(name,out path))
        {
            Debug.Log("名称错误");return null;
        }
        GameObject panelPrefab = null;
        //不在缓存中，就实例化
        if(!prefabDic.TryGetValue(name,out panelPrefab))
        {
            string Path = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(Path);
            prefabDic.Add(name, panelPrefab);//加入缓存字典，以便下次直接使用
        }
        GameObject panelObject=GameObject.Instantiate(panelPrefab,Root,false);
        panel=panelObject.GetComponent<BasePanel>();
        panelDic.Add(name, panel);//缓存实例
        panel.OpenPanel();//调用其显示面板的方法
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if(!panelDic.TryGetValue(name,out panel))
        {
            Debug.Log("界面没打开");return false;
        }
        panel.ClosePanel();
        return true;
    }

}

public class UIContants
{
    public const string Main = nameof(Main);
    public const string Character=nameof(Character);
    public const string Missions=nameof(Missions);
    public const string Shop=nameof(Shop);
    public const string Setting=nameof(Setting);
}