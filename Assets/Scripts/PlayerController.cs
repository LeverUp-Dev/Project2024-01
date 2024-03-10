using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inside Component")]
    CharacterController controller;
    Animator animator;

    [Header("Player reference")]
    public TilePathManager tilePathManager;
    public PlayerPhysics physics;

    public int diceNum;
    public int currentTile;

    public float rotationSpeed;

    Vector3 targetPos;
    bool isMove = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("StepTileMove");
        }
    }

    //Ÿ�� Ÿ���� �����ϰ� ������ ����
    public void WalkToTargetTile(int targetTile)
    {
        targetPos = new Vector3(tilePathManager.managerTiles[targetTile].mainTiles.position.x,
           transform.position.y,
           tilePathManager.managerTiles[targetTile].mainTiles.position.z);

        isMove = true;
    }

    // �� ���� �� �����̰��ϴ� �ڷ�ƾ �Լ�
    IEnumerator StepTileMove()
    {
        for(int i = 0; i < diceNum; i++)
        {
            WalkToTargetTile(currentTile + 1);

            yield return new WaitUntil(() => !isMove);

            yield return new WaitForSeconds(0.5f);

            currentTile++;
        }
    }
}
