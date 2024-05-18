using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class Lighting : MonoBehaviour
{
    public Color triggerColor = Color.red;
    public float innerRadius = 3f;
    public float outerRadius = 5f;
    public bool colorRave = false;
    public float colorChangeDuration = 1f;
    public float colorStayDuration = 1f;
    public float lightIntensity = 1.0f;
    public bool runtime = true;

    private float nextHue;
    private float transitionProgress = 0f;
    private float colorStayProgress = 0f;
    public List<GameObject> excludedObjects = new List<GameObject>();
    public List<GameObject> excludedParentObjects = new List<GameObject>();
    public List<GameObject> alwaysUpdateObjects = new List<GameObject>();
    public static List<Lighting> AllLightings = new List<Lighting>();


    private void Awake()
    {
        if (!AllLightings.Contains(this))
        {
            AllLightings.Add(this);
        }
    }

    private void OnDestroy()
    {
        AllLightings.Remove(this);
    }

    private void Update()
    {
        if (runtime || alwaysUpdateObjects.Count > 0)
        {
            if (colorRave)
            {
                ColorRaveHandler();
            }
        }
    }
    public bool IsExcluded(GameObject obj)
    {
        if (excludedObjects.Contains(obj))
            return true;

        Transform parent = obj.transform;
        while (parent != null)
        {
            if (excludedParentObjects.Contains(parent.gameObject))
                return true;
            parent = parent.parent;
        }

        return false;
    }

    private void ColorRaveHandler()
    {
        colorStayProgress += Time.deltaTime;
        if (colorStayProgress >= colorStayDuration)
        {
            TransitionHandler();
        }
    }

    private void TransitionHandler()
    {
        transitionProgress += Time.deltaTime / colorChangeDuration;
        if (transitionProgress >= 1f)
        {
            TransitionCompleteHandler();
        }
        else
        {
            ColorLerp();
        }
    }

    private void TransitionCompleteHandler()
    {
        triggerColor = Color.HSVToRGB(nextHue, 1f, 1f);
        nextHue = Random.value;
        transitionProgress = 0f;
        colorStayProgress = 0f;
    }

    private void ColorLerp()
    {
        float currentHue, currentSat, currentValue;
        Color.RGBToHSV(triggerColor, out currentHue, out currentSat, out currentValue);
        triggerColor = Color.HSVToRGB(Mathf.Lerp(currentHue, nextHue, transitionProgress), 1f, 1f);
    }

    public Color GetColorAtPosition(Vector3 position)
    {
        return CalculateColorBasedOnPosition(position);
    }

    private Color CalculateColorBasedOnPosition(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        if (distance <= innerRadius)
        {
            return triggerColor * lightIntensity;
        }
        else if (distance <= outerRadius)
        {
            float lerpValue = 1f - ((distance - innerRadius) / (outerRadius - innerRadius));
            return Color.Lerp(Color.white, triggerColor, lerpValue) * lightIntensity;
        }
        else
        {
            return Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
    public float CalculateInfluence(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        if (distance <= innerRadius)
        {
            return 1f;
        }
        else if (distance <= outerRadius)
        {
            return 1f - ((distance - innerRadius) / (outerRadius - innerRadius));
        }
        else
        {
            return 0f;
        }
    }
}
