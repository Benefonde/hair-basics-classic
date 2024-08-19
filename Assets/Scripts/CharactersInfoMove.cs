using System.Collections;
using UnityEngine;

public class CharactersInfoMove : MonoBehaviour
{
    void Start()
    {
        h = 0;
        y = 0;
        onRegular = true;
        if (charInfo.localPosition.x == 0)
        {
            left.SetActive(false);
        }
        if (charInfo.localPosition.x == -8580)
        {
            right.SetActive(false);
        }
        upDown.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        if (onRegular)
        {
            upDown.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }

    public void StartMoveH(int dir1)
    {
        StartCoroutine(Move(new Vector2(dir1, 0)));
    }
    public void StartMoveV()
    {
        if (onRegular)
        {
            StartCoroutine(Move(new Vector2(0, -1)));
        }
        else
        {
            StartCoroutine(Move(new Vector2(0, 1)));
        }
        onRegular = !onRegular;
    }

    IEnumerator Move(Vector2 direction)
    {
        h = charInfo.localPosition.x;
        y = charInfo.localPosition.y;
        left.SetActive(false);
        right.SetActive(false);
        upDown.gameObject.SetActive(false);
        charInfo.localPosition = new Vector2(h + direction.x * -780, y + direction.y * -480);
        yield return new WaitForSeconds(0.2f);
        if (charInfo.localPosition.x != 0)
        {
            left.SetActive(true);
        }
        if ((charInfo.localPosition.x != -8580 && onRegular) || (charInfo.localPosition.x != -4680 && !onRegular))
        {
            right.SetActive(true);
        }
        if (charInfo.localPosition.x >= -4680)
        {
            upDown.gameObject.SetActive(true);
        }
        upDown.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        if (onRegular)
        {
            upDown.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }

    public RectTransform charInfo;

    bool onRegular;

    float h;
    float y;

    public GameObject left;
    public GameObject right;
    public RectTransform upDown;
}
