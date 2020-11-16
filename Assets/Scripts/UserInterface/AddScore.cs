using UnityEngine;

public class AddScore : MonoBehaviour
{
    private void Start() => Destroy(this.gameObject, this.gameObject.GetComponent<Animation>().GetClip("IncreaseScore").length);
}
