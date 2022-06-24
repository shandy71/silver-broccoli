
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

public class BuyButton : UdonSharpBehaviour
{
    public MeshRenderer meshRender;
    public Material materialOver;
    public Material materialExit;
    public ParticleSystem pS;
    public ParticleSystem wakPS;
    public AudioSource aS;
    public Text buyText;
    public BoxCollider questionCollider;
    public GameObject buyBackScreen;
    public UdonBehaviour questionButton;

    //private bool isInTrigger;
    [UdonSynced] public bool buyIsPressed;
    private bool isGeneralMode;
    private bool isWakMode;

    public override void Interact()
    {
        if (buyIsPressed == false)
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "TurnToTrue");
            SendCustomNetworkEvent(NetworkEventTarget.All, "ChangeMatMouseOver");
            SendCustomNetworkEvent(NetworkEventTarget.All, "LightsSwitch");
            SendCustomNetworkEvent(NetworkEventTarget.All, "BuyDisplay");
            SendCustomNetworkEvent(NetworkEventTarget.All, "BlockQuestionButton");
        }
    }
    /*
    public void OnMouseDown()
    {
        if(buyIsPressed == false)
        {
            buyIsPressed = true;
            SendCustomNetworkEvent(NetworkEventTarget.All, "ChangeMatMouseOver");
            SendCustomNetworkEvent(NetworkEventTarget.All, "LightsSwitch");
            SendCustomNetworkEvent(NetworkEventTarget.All, "BuyDisplay");
        }
    }
    */

    public void LightsSwitch()
    {
        //lightOff.SetActive(!lightOff.activeInHierarchy);
        if(isGeneralMode == true)
        {
            pS.Play();
        }
        if(isWakMode == true)
        {
            wakPS.Play();
        }
        aS.Play();
    }
    public void BuyDisplay()
    {
        questionButton.SendCustomNetworkEvent(NetworkEventTarget.All, "TextToNull");
        buyBackScreen.SetActive(true);
        SendCustomNetworkEvent(NetworkEventTarget.All, "TextToBuy");
    }
    public void DisplayNone()
    {
        ChangeMatMouseExit();
        buyText.text = "";
        buyBackScreen.SetActive(false);
    }
    public void BlockQuestionButton()
    {
        questionCollider.enabled = false;
    }
    public void ChangeMatMouseOver()
    {
        meshRender.material = materialOver;
    }
    public void ChangeMatMouseExit()
    {
        meshRender.material = materialExit;
    }

    public void TurnToTrue()
    {
        buyIsPressed = true;
    }
    public void TextToBuy()
    {
        buyText.text = "BUY";
    }
}
