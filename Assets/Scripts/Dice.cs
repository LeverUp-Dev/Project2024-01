using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public PlayerController playerCtrl;

    public int diceNum;

    void Update()
    {
        //������Ʈ Ȱ��ȭ �� �ֻ��� ������
        if (gameObject.activeSelf)
            RollDice();
    }

    public void RollDice()
    {
        diceNum = Random.Range(1, 15);
    }
}
