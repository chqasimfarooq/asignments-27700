using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [SerializeField]
    private GameObject punchSlasher;
    private playerController GetPlayer;

    private void Awake()
    {
        GetPlayer = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<playerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.Player_Tag && !GetPlayer.isDefense)
        {
            AudioController.audioControler.PlaySound("PunchHit");
            Instantiate(punchSlasher, new Vector3(transform.position.x, transform.position.y, -4.0f),
            Quaternion.identity);
        }

        if (collision.tag == Tags.Enemy_Tag)
        {
            AudioController.audioControler.PlaySound("PunchHit");
            Instantiate(punchSlasher, new Vector3(transform.position.x, transform.position.y, -4.0f),
            Quaternion.identity);
        }
    }
}
