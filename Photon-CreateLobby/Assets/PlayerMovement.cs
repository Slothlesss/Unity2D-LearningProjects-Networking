using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    float dir;
    PhotonView view;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            dir = Input.GetAxisRaw("Horizontal") * speed;

            if (dir > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dir * Time.fixedDeltaTime, rb.velocity.y);
    }
}
