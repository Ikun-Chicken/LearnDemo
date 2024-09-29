using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��Ϸ�����࣬���ڹ�����Ϸ�е����ݣ����ң���ұ���
/// TODO��Ŀǰ��ͼ�����������һ��
/// </summary>
public class GameAttribute : MonoBehaviour
{
	public int Coin {  get; private set; }//���

	public int Multiply { get; private set; } = 1;//��ұ���

	public static GameAttribute instance;

	public int Life { get; private set; } = 1;//ʣ��������
	public int Init_life {  get; private set; } = 1;//��ʼ������

	public Text Text_Coin;//������UI

	public bool SoundOn { get; private set; } = true;//�����Ƿ���,TODO������Ӧ����Ҫ������һ����Ϸ������
	
	void Start () {
        Coin = 0;
        instance = this;
	}
	/// <summary>
	/// ����
	/// </summary>
    public void Reset()
    {
        Life = Init_life;
        Coin = 0;
        Multiply = 1;
    }
	void Update()
	{
		Text_Coin.text = Coin.ToString();//TODO������Ӧ�ò���Ҫÿ֡���£���Ҫʹ���¼�������������ʽ
	}

	public void AddCoin(int coinNumber)
	{
		Coin += Multiply * coinNumber;//����*�Ե��Ľ�Ҹ���
	}
}
