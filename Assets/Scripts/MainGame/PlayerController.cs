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
    
    //주사위
    public bool isReady = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //플레이어 가야할 타일의 방향 계산
    Vector3 CalculateMoveVector()
    {
        Vector3 dir = targetPos - transform.position;

        return dir;
    }

    //거리 판정으로 움직임을 멈추기
    void MoveToTile()
    {
        //목표 위치 도달 시 이동 중지
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            isMove = false;
        }
    }

    // 다음 이동 타일 기준으로 동작
    void RotationToTile()
    {
        Vector3 targetDir = CalculateMoveVector();

        Quaternion targetRot = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    void Update()
    {
        // 걷는 애니메이션은 isMove 변수에 맞춰 움직이게 한다.
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

        //스페이스 키를 눌러 다이스 돌림
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

    //타겟 타일을 지정하고 움직임 시작
    void WalkToTargetTile(int targetTile)
    {
        targetPos = new Vector3(tilePathManager.GetTilePath(targetTile).position.x,
           transform.position.y,
           tilePathManager.GetTilePath(targetTile).position.z);

        isMove = true;
    }

    // 한 걸음 씩 움직이게하는 코루틴 함수
    IEnumerator StepTileMove(int steps, float delay)
    {
        yield return new WaitForSeconds(delay);

        for(int i = 0; i < steps; i++)
        {
            //메인 경로일 때 로직
            if (tilePathManager.path == pathType.main)
            {
                //경로 전환 타일 판정
                if (tilePathManager.GetMainTilePath()[currentTile].isChoiceTile)
                {
                    GameObject choiceUI = tilePathManager.GetTilePath(currentTile).GetChild(0).gameObject;

                    choiceUI.SetActive(true);

                    yield return new WaitUntil(() => !choiceUI.activeSelf);
                }

                //이동
                WalkToTargetTile((currentTile + 1) % 40);

                yield return new WaitUntil(() => !isMove);

                yield return new WaitForSeconds(0.1f);

                if (currentTile == 39)
                    currentTile = 0;
                else
                    currentTile++;
            }
            //H경로일 때 로직
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
            //V경로일 때 로직
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
