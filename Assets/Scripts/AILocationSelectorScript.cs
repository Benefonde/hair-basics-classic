using UnityEngine;

public class AILocationSelectorScript : MonoBehaviour
{
	public Transform[] newLocation;

	public AmbienceScript ambience;

	private int id;

	public bool ambiencePlay;

	public int roomRemove = 24;

	public void GetNewTarget()
	{
		id = Mathf.RoundToInt(Random.Range(0f, newLocation.Length - 1));
		//id = Mathf.RoundToInt(Random.Range(46f, 48f));
		base.transform.position = newLocation[id].position;
		if (ambiencePlay)
		{
			ambience.PlayAudio();
		}
	}

	public void GetNewTargetHallway()
	{
		id = Mathf.RoundToInt(Random.Range(0f, newLocation.Length - 24));
		base.transform.position = newLocation[id].position;
		if (ambiencePlay)
		{
			ambience.PlayAudio();
		}
	}
	public Vector3 GetNewTargetQuick(bool hallway)
	{
		int a = 0;
		if (hallway)
        {
			a = Mathf.RoundToInt(Random.Range(0f, newLocation.Length - 24));
		}
        else
		{
			a = Mathf.RoundToInt(Random.Range(0f, newLocation.Length - 1));
		}
		return newLocation[a].position;
	}

	public void QuarterExclusive()
	{
		id = Mathf.RoundToInt(Random.Range(1f, newLocation.Length - 24));
		base.transform.position = newLocation[id].position;
	}
}
