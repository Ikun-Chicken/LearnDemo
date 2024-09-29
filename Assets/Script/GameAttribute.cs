using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游戏属性类，用于管理游戏中的数据，如金币，金币倍数
/// TODO：目前视图和数据耦合在一起
/// </summary>
public class GameAttribute : MonoBehaviour
{
	public int Coin {  get; private set; }//金币

	public int Multiply { get; private set; } = 1;//金币倍数

	public static GameAttribute instance;

	public int Life { get; private set; } = 1;//剩余生命数
	public int Init_life {  get; private set; } = 1;//初始生命数

	public Text Text_Coin;//界面金币UI

	public bool SoundOn { get; private set; } = true;//声音是否开启,TODO：这里应该需要有另外一个游戏设置类
	
	void Start () {
        Coin = 0;
        instance = this;
	}
	/// <summary>
	/// 重玩
	/// </summary>
    public void Reset()
    {
        Life = Init_life;
        Coin = 0;
        Multiply = 1;
    }
	void Update()
	{
		Text_Coin.text = Coin.ToString();//TODO：这里应该不需要每帧更新，需要使用事件，或者其他方式
	}

	public void AddCoin(int coinNumber)
	{
		Coin += Multiply * coinNumber;//倍数*吃到的金币个数
	}
}
