# Speech Recognition in Unity
Speech (Voice) Recognition using C# and Unity3D.
* [Tutorial](//lightbuzz.com/speech-recognition-unity/)
* [Source Code](//github.com/LightBuzz/Speech-Recognition-Unity)

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

private void Update()
{
    var x = target.transform.position.x;
    var y = target.transform.position.y;

    switch (word)
    {
        case "up":
            y += speed;
            break;
        case "down":
            y -= speed;
            break;
        case "left":
            x -= speed;
            break;
        case "right":
            x += speed;
            break;
    }

    target.transform.position = new Vector3(x, y, 0);
}
```

## Exporting the project
KeywordRecognizer is available for Windows Standalone and Windows Store (Windows 8.1 or Windows 10):
* PC
* Phone
* XBOX
* HoloLens

## Contributors
* [Michail Moiropoulos](http://instoriumgames.com)
* [Vangos Pterneas](http://pterneas.com)
