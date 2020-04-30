using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

/// <summary>
/// added by shachar oz
/// inspired by the code here https://lightbuzz.com/speech-recognition-unity/
/// </summary>
public class SpeechRecognitionBase : MonoBehaviour
{
    public List<Phrase> phrases;
    
    public Text ResultedText;

    protected PhraseRecognizer phraseRecognizer;


    [System.Serializable]
    public class UnityEventString : UnityEngine.Events.UnityEvent<string> { };
    public UnityEventString OnPhraseRecognized;

    private void Start()
    {
        if (phrases != null)
        {
            phraseRecognizer = new KeywordRecognizer(Phrase.ConvertToStringArray(phrases), ConfidenceLevel.Low);
            phraseRecognizer.OnPhraseRecognized += SpeechRecognizer_OnPhraseRecognized;
            phraseRecognizer.Start();
        }

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void SpeechRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string word = args.text;
        ResultedText.text = "You said: <b>" + word + "</b>";

        OnPhraseRecognized.Invoke(word);
    }

    private void OnApplicationQuit()
    {
        if (phraseRecognizer != null && phraseRecognizer.IsRunning)
        {
            phraseRecognizer.OnPhraseRecognized -= SpeechRecognizer_OnPhraseRecognized;
            phraseRecognizer.Stop();
            phraseRecognizer.Dispose();
        }
    }
}

[System.Serializable]
public class Phrase
{
    public string target;


    public static string[] ConvertToStringArray(List<Phrase> _phrases)
    {
        string[] result = new string[_phrases.Count];
        
        for (int i=0; i<_phrases.Count; i++)
        {
            result[i] = _phrases[i].target;
        }

        return result;
    }
}