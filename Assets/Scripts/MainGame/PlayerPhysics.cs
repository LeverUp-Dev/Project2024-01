using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public float gravity = -9.81f;
    public float speed;

    CharacterController controller;
    Vector3 physicsDir;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!controller.isGrounded)
        {
            physicsDir.y += gravity * Time.deltaTime;
        }

        controller.Move(physicsDir * speed * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        physicsDir = new Vector3(direction.x, physicsDir.y, direction.z);
    }
}
