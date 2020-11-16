using UnityEngine;

public class BoosterBlock : BlockBase
{
    public float boost;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "Effector")
        {
            playerRigidbody.velocity *= new Vector2(boost, boost);
            playerController.canMove = true;
        }
    }
}
