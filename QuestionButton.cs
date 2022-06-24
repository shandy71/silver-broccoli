
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

public class QuestionButton : UdonSharpBehaviour
{
    public Text displayText;
    public MeshRenderer mR;
    public Material onMaterial;
    public Material offMaterial;
    public AudioSource questionSound;
    public GameObject qBox;
    public Animator qBoxAnimator;
    public GameObject screenText;

    [UdonSynced] private bool onOff = false;

    public override void Interact()
    {
        onOff = !onOff;

        if (onOff)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "Pressed");
        }
        else
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "TextToNull");
        }



    }
    public void Pressed()
    {
        displayText.text = "질문";
        mR.material = onMaterial;
        //소리가 변경될때까지 일단 해제
        questionSound.Play();

        QuestionBox();

        DisplayBackScreen();

    }

    public void TextToNull()
    {
        displayText.text = "";
        mR.material = offMaterial;

        qBox.SetActive(false);

        DeleteBackScreen();
    }
    public void QuestionBox()
    {
        qBox.SetActive(true);
        qBoxAnimator.SetTrigger("turnOn");
    }

    public void DisplayBackScreen()
    {
        screenText.SetActive(true);
    }

    public void DeleteBackScreen()
    {
        screenText.SetActive(false);
    }
}
