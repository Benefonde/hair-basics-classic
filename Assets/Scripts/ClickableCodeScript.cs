using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickableCodeScript : MonoBehaviour
{
    void Start()
    {
        int found = 0;
        codePieceFound = GetComponent<Animator>();
        if (PlayerPrefs.GetInt("piecesFound", 0) == 4)
        {
            code.SetActive(true);
        }
        if (PlayerPrefs.GetInt($"foundPiece{codePiece}", 0) == 1)
        {
            gameObject.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.GetInt($"foundPiece{i}", 0) == 1)
            {
                found++;
            }
        }
    }

    public void Clicked()
    {
        GetComponent<Button>().enabled = false;
        codePieceFound.SetTrigger("FOUND");
        PlayerPrefs.SetInt($"foundPiece{codePiece}", 1);
        int foundPieces = PlayerPrefs.GetInt("foundPiece1", 0) + PlayerPrefs.GetInt("foundPiece2", 0) + PlayerPrefs.GetInt("foundPiece3", 0) + PlayerPrefs.GetInt("foundPiece4", 0);
        PlayerPrefs.SetInt("piecesFound", foundPieces);
        PlayerPrefs.Save();
        globalAudioDevice.PlayOneShot(piecesFound[foundPieces - 1]);
        if (foundPieces == 4)
        {
            StartCoroutine(SendToEsteSecret());
        }
    }

    public IEnumerator SendToEsteSecret()
    {
        yield return new WaitForSeconds(piecesFound[3].length);
        SceneManager.LoadScene("EsteSecret");
    }

    public int codePiece;
    private Animator codePieceFound;
    public AudioSource globalAudioDevice;
    public AudioClip[] piecesFound;
    

    public GameObject code;
}
