using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using JetBrains.Annotations;

public class PlayerLength : NetworkBehaviour
{
    [SerializeField] private GameObject bodyBallPrefab;
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [CanBeNull] public static event System.Action<ushort> ChangedLengthEvent;

    private List<GameObject> _tails;
    private Transform _lastTail;
    private Collider2D _collider2D;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _tails = new List<GameObject>();
        _lastTail = transform;
        _collider2D = GetComponent<Collider2D>();
        if (!IsServer)
        {
            length.OnValueChanged += LengthChangedEvent;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        DestroyTails();
    }

    private void DestroyTails()
    { 
        while(_tails.Count != 0)
        {
            GameObject tail = _tails[0];
            _tails.RemoveAt(0);
            Destroy(tail);
        }
    }

    [ContextMenu("AddLength")]
    public void AddLength()
    {

        Debug.Log("2"); //Server will run this
        length.Value += 1;
        LengthChanged(); 
    }

    private void LengthChanged()
    {

        InstantiateTail();

        if (!IsOwner) return;
        ChangedLengthEvent?.Invoke(length.Value);

        ClientMusicPlayer.Instance.PlayNomAudioClip();
    }

    private void LengthChangedEvent(ushort previousValue, ushort newValue)
    {
        Debug.Log("1");  //Client will run this
        LengthChanged();
    }

    private void InstantiateTail()
    {
        GameObject tailGameObject = Instantiate(bodyBallPrefab, transform.position, Quaternion.identity);
        tailGameObject.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
        if (tailGameObject.TryGetComponent(out Tail tail))
        {
            tail.networkedOwner = transform;
            tail.followTransform = _lastTail;
            _lastTail = tailGameObject.transform;
            Physics2D.IgnoreCollision(tailGameObject.GetComponent<Collider2D>(), _collider2D);
        }
        _tails.Add(tailGameObject);
    }

}
