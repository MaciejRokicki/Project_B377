using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Player/Sprite", order = 1)]
public class PlayerSprite : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public Material material;
    public int cost;
}