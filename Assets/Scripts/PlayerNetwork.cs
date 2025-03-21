using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;  
public class PlayerNetwork : NetworkBehaviour
{
    private void Update()
    {
        if(!IsOwner) return;
        
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
        
        float moveSpeed = 5f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
