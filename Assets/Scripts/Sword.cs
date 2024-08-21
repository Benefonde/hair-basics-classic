using UnityEngine;

[CreateAssetMenu(fileName = "SwordType", menuName = "New Sword")]
public class Sword : ScriptableObject
{
    public int attack;
    public int durability;
    public int currentDurability;
    public Color color = Color.white;
    public bool trophyKriller;
}
