using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public PlayerController playerCtrl;

    public int diceNum;
    public float diceFalseDelay = 1.5f;

    Animator ani;

    bool isHit = false;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        //오브젝트 활성화 시 주사위 돌리기
        if (gameObject.activeSelf && !isHit)
        {
            RollDice();
        }
    }

    public void RollDice()
    {
        ani.StopRecording();

        diceNum = Random.Range(1, 15);
    }

    public void HitDice()
    {
        ani.enabled = false;

        isHit = true;

        StartCoroutine("FalseTheActive");
    }

    IEnumerator FalseTheActive()
    {
        yield return new WaitForSeconds(diceFalseDelay);

        gameObject.SetActive(false);

        isHit = false;

        ani.enabled = true;

        StopCoroutine("FalseTheActive");
    }
}
