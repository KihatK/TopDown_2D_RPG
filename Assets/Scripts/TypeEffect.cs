using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject endCursor;
    public int charPerSeconds;
    public bool isAnim;

    AudioSource audioSource;

    Text talkText;
    string targetMsg;
    int index;

    private void Awake() {
        talkText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg) {
        if (isAnim) {
            talkText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else {
            targetMsg = msg;

            EffectStart();
        }
    }

    void EffectStart() {
        talkText.text = "";
        index = 0;
        endCursor.SetActive(false);

        isAnim = true;

        Invoke("Effecting", (float)1 / charPerSeconds);
    }

    void Effecting() {
        if (talkText.text == targetMsg) {
            EffectEnd();
            return;
        }
        talkText.text += targetMsg[index];

        //Sound Effect
        if (targetMsg[index] != ' ' || targetMsg[index] != '.') {
            audioSource.Play();
        }

        index++;

        Invoke("Effecting", (float)1 / charPerSeconds);
    }

    void EffectEnd() {
        isAnim = false;
        endCursor.SetActive(true);
    }
}
