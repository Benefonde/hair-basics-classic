using UnityEngine;

[CreateAssetMenu(fileName = "SwordType", menuName = "New Sword")]
public class Sword : ScriptableObject
{
    public int attack;
    public int durability;
    public Color color = Color.white;
}
