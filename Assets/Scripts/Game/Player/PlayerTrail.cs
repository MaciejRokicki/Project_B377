using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "Player/Trail", order = 1)]
public class PlayerTrail : ScriptableObject
{
    public int id;
    public Material material;
    public LineTextureMode TextureMode;
    public int cost;
}