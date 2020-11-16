using UnityEngine;

public class KillerBlock : BlockBase
{
    public override void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player")
        {
            playerController.GameOver();
        }
    }
}
