using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PlayerMovemechanim : NetworkBehaviour
{
    Animator anim;
    public GameObject bulletPrefab;

    public override void OnStartLocalPlayer()
    {
        anim = GetComponent<Animator>();
    }

    [Command]
    void CmdWalk(bool isWalking, float x, float y)
    {
        //Debug.Log("CmdWalk");
        RpcWalk(isWalking, x, y);
    }

    [ClientRpc]
    void RpcWalk(bool isWalking, float x, float y)
    {
        //Debug.Log("RpcWalk");
        //anim = GetComponent<Animator>();
        anim.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
            //transform.Translate(x, y, 0);
            transform.position += new Vector3(x, y, 0).normalized * Time.deltaTime;

        }
    }



    //Command runs function on server
    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            transform.position + transform.right, //set var
            Quaternion.identity); // rotation

        bullet.GetComponent<Rigidbody2D>().velocity = transform.right * 4;

        //spawn on clients
        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        //anim = GetComponent<Animator>();
        var x = Input.GetAxisRaw("Horizontal") * 0.1f;
        var y = Input.GetAxisRaw("Vertical") * 0.1f;

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            bool isWalking = (Mathf.Abs(x) + Mathf.Abs(y)) > 0;
            CmdWalk(isWalking, x, y);
            //RpcWalk(isWalking, x, y);


        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
            Debug.Log("Firing");
        }
    }


}
