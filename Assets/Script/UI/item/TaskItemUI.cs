using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUI : RecyclingListViewItem
{
    //��������
    public TextMeshProUGUI desText;

    //ǰ����ť
    public Button goAheadBtn;
    //�콱����
    public Button getAwardBtn;
    //������
    public Slider progressSlider;
	// ����ͼ��
	public Image icon;
	// �������ͱ�ǣ�����/֧�� 
	public Image taskType;
}
