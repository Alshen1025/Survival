using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;

    [SerializeField] private float PosX;
    [SerializeField] private float PosY;
    [SerializeField] private float PosZ;

    [SerializeField] private float Speed = 2.0f;

    private void Start()
    {
        player = Player_Movement.instance.transform;

        Vector3 startPosition = new Vector3(player.transform.position.x + PosX,
            player.transform.position.y + PosY,
            player.transform.position.z + PosZ);
        transform.position = startPosition;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(
            player.transform.position.x + PosX,
            player.transform.position.y + PosY,
            player.transform.position.z + PosZ
            ), Time.deltaTime * Speed);
    }
}
