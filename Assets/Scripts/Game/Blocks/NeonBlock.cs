using UnityEngine;

public class NeonBlock : BlockBase
{
    public int neonBlocks = 1;

    public override void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name == "Player")
        {
            gameManager.IncreaseNeonBlocks(this.gameObject.transform.position, neonBlocks);
            
            base.OnCollisionEnter2D(coll);
        }
    }
}