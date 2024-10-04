using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���н���ĸ��࣬���н��涼�д򿪣��رյķ���
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
    /// �������ִ򿪶�Ӧ���
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
