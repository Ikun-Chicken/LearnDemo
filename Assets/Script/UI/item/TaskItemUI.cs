using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUI : RecyclingListViewItem
{
    //任务描述
    public TextMeshProUGUI desText;

    //前往按钮
    public Button goAheadBtn;
    //领奖暗流
    public Button getAwardBtn;
    //进度条
    public Slider progressSlider;
	// 任务图标
	public Image icon;
	// 任务类型标记，主线/支线 
	public Image taskType;
}
