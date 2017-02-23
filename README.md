# Speech Recognition in Unity
Speech (Voice) Recognition using C# and Unity3D.
* [Tutorial](http://lightbuzz.com/speech-recognition-unity/)
* [Source Code](https://github.com/LightBuzz/Speech-Recognition-Unity)

![Speech recognition game in Unity3D](http://lightbuzz.com/wp-content/uploads/2016/07/speech-recognition-unity-game-ui.jpg)

## Prerequisites
To run the project, you’ll need the following software components:
* Unity3D 5.5+
* Visual Studio 2015+
* Windows 10

## Source Code
We'll be using the KeywordRecognizer class to detect the voice commands:

```
private KeywordRecognizer recognizer;

private void Start()
{
    if (keywords != null)
    {
        recognizer = new KeywordRecognizer(keywords, confidence);
        recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
        recognizer.Start();
    }
}

private void OnApplicationQuit()
{
    if (recognizer != null && recognizer.IsRunning)
    {
        recognizer.Stop();
    }
}

private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
{
    word = args.text;
    results.text = "You said: <b>" + word + "</b>";
}
```

## Exporting the project
KeywordRecognizer is available for Windows Standalone and Windows Store (Windows 8.1 or Windows 10):
* PC
* Phone
* XBOX
* HoloLens

## Contributors
The project is brought to you by [LightBuzz](http://lightbuzz.com)
* [Michail Moiropoulos](http://instoriumgames.com)
* [Vangos Pterneas](http://pterneas.com)
