using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public PlayerController playerCtrl;

    public int diceNum;

    void Update()
    {
        //오브젝트 활성화 시 주사위 돌리기
        if (gameObject.activeSelf)
            RollDice();
    }

    public void RollDice()
    {
        diceNum = Random.Range(1, 15);
    }
}
