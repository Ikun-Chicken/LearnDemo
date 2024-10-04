using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> 
{
    //ui���ֶ�Ӧui������·��
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
    //Ԥ�Ƽ���Դ�Ļ��棬�����ȡ��Դ
    private Dictionary<string, GameObject> prefabDic=new Dictionary<string, GameObject>();
    //�Ѿ��򿪵Ľ����ʵ���Ļ��棬��������
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
    /// ����Ҫ�򿪵Ľ��棬�����������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        if(panelDic.TryGetValue(name, out panel))
        {
            Debug.Log("�Ѿ���"); return null;
		}
        string path = "";
        //�����������ȥ�ֵ��ҵ���Ӧ·��
        if(!pathDic.TryGetValue(name,out path))
        {
            Debug.Log("���ƴ���");return null;
        }
        GameObject panelPrefab = null;
        //���ڻ����У���ʵ����
        if(!prefabDic.TryGetValue(name,out panelPrefab))
        {
            string Path = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(Path);
            prefabDic.Add(name, panelPrefab);//���뻺���ֵ䣬�Ա��´�ֱ��ʹ��
        }
        GameObject panelObject=GameObject.Instantiate(panelPrefab,Root,false);
        panel=panelObject.GetComponent<BasePanel>();
        panelDic.Add(name, panel);//����ʵ��
        panel.OpenPanel();//��������ʾ���ķ���
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if(!panelDic.TryGetValue(name,out panel))
        {
            Debug.Log("����û��");return false;
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