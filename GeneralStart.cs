
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.Udon.Common.Interfaces;

public class GeneralStart : UdonSharpBehaviour
{
    public UdonBehaviour nextButtonUB;
    public UdonBehaviour[] buyButtons;
    public Material changeButtonLight;
    public Material basicButtonColor;
    public MeshRenderer mR;
    public UdonBehaviour wakModeUB;
    [UdonSynced] private string cardArray = "PropArray";
    [UdonSynced] private int[] rawIntArray;

    public void Start()
    {
        rawIntArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
    }
    public override void Interact()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, "ShuffleIntArray");
        SendCustomNetworkEvent(NetworkEventTarget.All, "ChangeMaterial");
    }
    public void ShuffleIntArray()
    {
        Utilities.ShuffleArray(rawIntArray);
        nextButtonUB.SetProgramVariable("cardIntArray", rawIntArray);
        nextButtonUB.SetProgramVariable("isGeneral", true);
        nextButtonUB.SetProgramVariable("isWak", false);
        for(int i = 0; i < buyButtons.Length; i++)
        {
            buyButtons[i].SetProgramVariable("isGeneralMode", true);
            buyButtons[i].SetProgramVariable("isWakMode", false);
        }
    }

    public void ChangeMaterial()
    {
        mR.material = changeButtonLight;
        wakModeUB.SendCustomNetworkEvent(NetworkEventTarget.All, "ReturnMaterial");
    }
    public void ReturnMaterial()
    {
        mR.material = basicButtonColor;
    }
}
