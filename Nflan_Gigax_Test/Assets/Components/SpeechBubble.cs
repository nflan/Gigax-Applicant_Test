using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public TMP_Text speechText;
    public Transform npcTransform;

    void start()
    {
        HideSpeech();
    }

    void Update()
    {
        Vector3 offset = new Vector3(0f, 2f, 0f); // Put the speechbubble above NPC.
        transform.position = npcTransform.position + offset;
    }

    // Set content of the speech and activate the component.
    public void ShowSpeech(string speech)
    {
        speechText.text = speech;
        gameObject.SetActive(true);
    }

    // Desactivate the component which will continue following the agent but hidden.
    public void HideSpeech()
    {
        gameObject.SetActive(false);
    }
}
