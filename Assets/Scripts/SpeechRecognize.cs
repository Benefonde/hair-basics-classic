using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows.Speech;
using System.Linq;

public class SpeechRecognize : MonoBehaviour
{
    /*KeywordRecognizer keywordRecognizer;
    DictationRecognizer dictationRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public GameControllerScript gc;

    void Start()
    {
        keywords.Clear();
        keywords.Add("evil leafy", () =>
        {
            print("ay yo i heard you say EVIL LEAFY so here is EVIL LEAFY");
            gc.SpawnEvilLeafy();
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        /*
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
        dictationRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        print("aw yum " + confidence);
    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        print(text);
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        print("aw yuck " + error);
    }
    */
}
