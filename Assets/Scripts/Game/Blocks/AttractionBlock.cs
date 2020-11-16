using UnityEngine;

public class AttractionBlock : BlockBase
{

    private PlayerController player;

    public override void Awake()
    {
        base.Awake();
        this.player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.name == "Effector")
            player.canMove = true;
    }
}
