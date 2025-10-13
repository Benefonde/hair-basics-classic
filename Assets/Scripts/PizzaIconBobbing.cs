using UnityEngine;
public class PizzaIconBobbing : MonoBehaviour
{
    private void Start()
    {
        offset = GetComponent<RectTransform>().anchoredPosition;
    }
    void Update()
    {
        time += Time.deltaTime;
        Vector3 uh = new Vector3(offset.x, offset.y + Mathf.Sin(time * speed) * maxAmplitude, offset.z);
        if (PixelPerfect)
        {
            uh = new Vector3(Mathf.Round(uh.x), Mathf.Round(uh.y), Mathf.Round(uh.z));
        }
        transform.localPosition = uh;
        if (UI)
        {
            GetComponent<RectTransform>().anchoredPosition = uh;
        }
    }
    public float maxAmplitude;
    public float speed = 1;
    public Vector3 offset;
    float time;
    public bool PixelPerfect;
    public bool UI;
}