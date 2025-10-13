using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<string> text;
    public List<Sprite> sprites;
    public List<AudioClip> blips;
}
