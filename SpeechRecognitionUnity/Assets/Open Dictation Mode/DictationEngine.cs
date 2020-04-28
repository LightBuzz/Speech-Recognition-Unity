using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

/// <summary>
/// dictation mode is the mode where the PC tries to recognize the speech with out any assistnace or guidance. it is the most clear way.
/// 
/// Hypotethis are thrown super fast, but could have mistakes.
/// Resulted complete phrase will be determined once the person stops speaking. the best guess from the PC will go on the result.
/// 
/// for grammer controls and ruels see here
/// https://www.w3.org/TR/speech-grammar/
/// 
/// added by shachar oz
/// </summary>
public class DictationEngine : MonoBehaviour
{
    public Text ResultedText;

    protected DictationRecognizer dictationRecognizer;

    [System.Serializable]
    public class UnityEventString : UnityEngine.Events.UnityEvent<string> { };
    public UnityEventString OnPhraseRecognized;

    public UnityEngine.Events.UnityEvent OnUserStartedSpeaking;

    private bool isUserSpeaking;

    void Start()
    {
        StartDictationEngine();
    }

    /// <summary>
    /// Hypotethis are thrown super fast, but could have mistakes.
    /// </summary>
    /// <param name="text"></param>
    private void DictationRecognizer_OnDictationHypothesis(string text)
    {
        Debug.LogFormat("Dictation hypothesis: {0}", text);

        if (isUserSpeaking == false)
        {
            isUserSpeaking = true;
            OnUserStartedSpeaking.Invoke();
        }
    }

    /// <summary>
    /// thrown when engine has some messages, that are not specifically errors
    /// </summary>
    /// <param name="completionCause"></param>
    private void DictationRecognizer_OnDictationComplete(DictationCompletionCause completionCause)
    {
        if (completionCause != DictationCompletionCause.Complete)
        {
            Debug.LogWarningFormat("Dictation completed unsuccessfully: {0}.", completionCause);


            switch (completionCause)
            {
                case DictationCompletionCause.TimeoutExceeded:
                case DictationCompletionCause.PauseLimitExceeded:
                    //we need a restart
                    CloseDictationEngine();
                    StartDictationEngine();
                    break;

                case DictationCompletionCause.UnknownError:
                case DictationCompletionCause.AudioQualityFailure:
                case DictationCompletionCause.MicrophoneUnavailable:
                case DictationCompletionCause.NetworkFailure:
                    //error without a way to recover
                    CloseDictationEngine();
                    break;

                case DictationCompletionCause.Canceled:
                    //happens when focus moved to another application 

                case DictationCompletionCause.Complete:
                    CloseDictationEngine();
                    StartDictationEngine();
                    break;
            }
        }
    }

    /// <summary>
    /// Resulted complete phrase will be determined once the person stops speaking. the best guess from the PC will go on the result.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="confidence"></param>
    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.LogFormat("Dictation result: {0}", text);
        if (ResultedText) ResultedText.text += text + "\n";

        

        if (isUserSpeaking == true)
        {
            isUserSpeaking = false;
            OnPhraseRecognized.Invoke(text);
        }
    }

    private void DictationRecognizer_OnDictationError(string error, int hresult)
    {
        Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
    }


    private void OnApplicationQuit()
    {
        CloseDictationEngine();
    }

    private void StartDictationEngine()
    {
        isUserSpeaking = false;

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;

        dictationRecognizer.Start();
    }

    private void CloseDictationEngine()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis;
            dictationRecognizer.DictationComplete -= DictationRecognizer_OnDictationComplete;
            dictationRecognizer.DictationResult -= DictationRecognizer_OnDictationResult;
            dictationRecognizer.DictationError -= DictationRecognizer_OnDictationError;

            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
                dictationRecognizer.Stop();
            
            dictationRecognizer.Dispose();
        }
    }
}