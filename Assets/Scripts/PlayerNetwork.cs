using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedobjectTransform;
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
    {
        _int = 56,
        _bool = true
    },NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }
    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += ((MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        });
    }
    private void Update()
    {
       
        if(!IsOwner) return;
        
        if(Input.GetKeyDown(KeyCode.T))
        {
          spawnedobjectTransform =   Instantiate(spawnedObjectPrefab);
            spawnedobjectTransform.GetComponent<NetworkObject>().Spawn();
            // TestServerRpc();
            // TestClientRpc();
            // randomNumber.Value =new MyCustomData{_int = 64,_bool = false,message = "Hello"};
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {      spawnedobjectTransform.GetComponent<NetworkObject>().Despawn();
            // Destroy(spawnedobjectTransform.gameObject);
        }
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
        
        float moveSpeed = 5f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    void TestServerRpc()
    {
        Debug.Log("Test ServerRpc " + OwnerClientId );
    }
    [ClientRpc]
    void TestClientRpc()
    {
        Debug.Log("Test ClientRpc ");
    }
}
