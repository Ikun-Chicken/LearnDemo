using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有界面的父类，所有界面都有打开，关闭的方法
/// </summary>
public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false;
    protected string panelName;
    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    /// <summary>
    /// 传入名字打开对应面板
    /// </summary>
    /// <param name="panelName"></param>
    public virtual void OpenPanel()
    {
        SetActive(true);
    }

    public void ClosePanel()
    {
        isRemove = true;
        SetActive(false);
        UIManager.Instance.panelDic.Remove(panelName);
        Destroy(gameObject);
    }
}
