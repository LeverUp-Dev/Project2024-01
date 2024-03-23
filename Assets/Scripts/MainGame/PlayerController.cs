using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inside Component")]
    Animator animator;

    [Header("Player reference")]
    public TilePathManager tilePathManager;
    public PlayerPhysics physics;
    public Dice dice;

    public int currentTile;

    public float rotationSpeed;

    Vector3 targetPos;
    bool isMove = false;
    
    //�ֻ���
    public bool isReady = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //�÷��̾� ������ Ÿ���� ���� ���
    Vector3 CalculateMoveVector()
    {
        Vector3 dir = targetPos - transform.position;

        return dir;
    }

    //�Ÿ� �������� �������� ���߱�
    void MoveToTile()
    {
        //��ǥ ��ġ ���� �� �̵� ����
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            isMove = false;
        }
    }

    // ���� �̵� Ÿ�� �������� ����
    void RotationToTile()
    {
        Vector3 targetDir = CalculateMoveVector();

        Quaternion targetRot = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    void Update()
    {
        // �ȴ� �ִϸ��̼��� isMove ������ ���� �����̰� �Ѵ�.
        animator.SetBool("isWalk", isMove);

        if (isMove)
        {
            physics.MoveTo(CalculateMoveVector().normalized);
            RotationToTile();
            MoveToTile();
        }
        else
        {
            physics.MoveTo(Vector3.zero);
        }

        //�����̽� Ű�� ���� ���̽� ����
        if (Input.GetKeyDown(KeyCode.Space) && isReady)
        {
            isReady = false;

            dice.HitDice();

            StartCoroutine(StepTileMove(dice.diceNum, dice.diceFalseDelay));
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReady = !isReady;
            dice.gameObject.SetActive(isReady);
        }
    }

    //Ÿ�� Ÿ���� �����ϰ� ������ ����
    void WalkToTargetTile(int targetTile)
    {
        targetPos = new Vector3(tilePathManager.GetTilePath(targetTile).position.x,
           transform.position.y,
           tilePathManager.GetTilePath(targetTile).position.z);

        isMove = true;
    }

    // �� ���� �� �����̰��ϴ� �ڷ�ƾ �Լ�
    IEnumerator StepTileMove(int steps, float delay)
    {
        yield return new WaitForSeconds(delay);

        for(int i = 0; i < steps; i++)
        {
            //���� ����� �� ����
            if (tilePathManager.path == pathType.main)
            {
                //��� ��ȯ Ÿ�� ����
                if (tilePathManager.GetMainTilePath()[currentTile].isChoiceTile)
                {
                    GameObject choiceUI = tilePathManager.GetTilePath(currentTile).GetChild(0).gameObject;

                    choiceUI.SetActive(true);

                    yield return new WaitUntil(() => !choiceUI.activeSelf);
                }

                //�̵�
                WalkToTargetTile((currentTile + 1) % 40);

                yield return new WaitUntil(() => !isMove);

                yield return new WaitForSeconds(0.1f);

                if (currentTile == 39)
                    currentTile = 0;
                else
                    currentTile++;
            }
            //H����� �� ����
            else if(tilePathManager.path == pathType.h)
            {
                if (tilePathManager.GetHTilePath()[currentTile].isChoiceTile)
                {
                    GameObject choiceUI = tilePathManager.GetTilePath(currentTile).GetChild(0).gameObject;

                    choiceUI.SetActive(true);

                    yield return new WaitUntil(() => !choiceUI.activeSelf);
                }

                if (currentTile + 1 < 9)
                {
                    WalkToTargetTile((currentTile + 1));
                }
                else if(currentTile + 1 == 9)
                {
                    tilePathManager.path = pathType.main;

                    currentTile = 24;

                    WalkToTargetTile(currentTile + 1);
                }

                yield return new WaitUntil(() => !isMove);

                yield return new WaitForSeconds(0.1f);

                currentTile++;
            }
            //V����� �� ����
            else if(tilePathManager.path == pathType.v)
            {
                if (tilePathManager.GetVTilePath()[currentTile].isChoiceTile)
                {
                    GameObject choiceUI = tilePathManager.GetTilePath(currentTile).GetChild(0).gameObject;

                    choiceUI.SetActive(true);

                    yield return new WaitUntil(() => !choiceUI.activeSelf);
                }

                if (currentTile + 1 < 9)
                {
                    WalkToTargetTile((currentTile + 1));
                }
                else if (currentTile + 1 == 9)
                {
                    tilePathManager.path = pathType.main;

                    currentTile = 14;

                    WalkToTargetTile(currentTile + 1);
                }

                yield return new WaitUntil(() => !isMove);

                //yield return new WaitForSeconds(0.1f);

                currentTile++;
            }
        }

        yield break;
    }
}
