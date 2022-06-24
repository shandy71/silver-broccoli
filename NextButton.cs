
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NextButton : UdonSharpBehaviour
{
    public GameObject[] cardGeneralArray;
    public GameObject[] cardWakArray;
    public GameObject spawnTarget;
    public GameObject currentCard;
    public GameObject nextButton;
    public Vector3 cardScale;
    public Text t;
    public Animator boxAnimator;
    public UdonBehaviour[] buyButtonUdonBehaviour;
    public UdonBehaviour[] questionButtonUB;
    public UdonBehaviour timerUB;
    public AudioSource boxSound;
    public BoxCollider bC;
    public BoxCollider[] questionBoxCollider;
    public UdonBehaviour counterUB;
    public UdonBehaviour[] findTeamNumber;
    public VRCPlayerApi pressedPlayer;
    public Text leftNumberOfCards;
    public int countOfCards;
    public GameObject owner;

    
    private int playerTeamNumber;
    [UdonSynced] private string playerTag;
    [UdonSynced] public int[] cardIntArray;
    [UdonSynced] public bool isGeneral = false;
    [UdonSynced] public bool isWak = false;
    [UdonSynced] private int count;
    [UdonSynced] public bool isCardExist;
    [UdonSynced] public string currentCardName;

    private int preCount = -1;

    public void Start()
    {
        //Networking.SetOwner(Networking.GetOwner(owner), this.gameObject);
        OnCountChanged();
    }
    public override void Interact()
    {
        // 여기서 count가 최대수와 같으면 그냥 코드 시작안하게
        //SendCustomNetworkEvent(NetworkEventTarget.All, "FindPlayer2");
        SendCustomNetworkEvent(NetworkEventTarget.All, "DisableCollider");
        SendCustomNetworkEvent(NetworkEventTarget.All, "BoxAnimation");
        SendCustomEventDelayedSeconds("SpawnCycle", 0.7f);
        //timerUB.SendCustomNetworkEvent(NetworkEventTarget.All, "ActivateTimerMethod");
    }
    public void SpawnCycle()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        IncrementCount();
        //SendCustomNetworkEvent(NetworkEventTarget.All, "DestroyCard");
        //SendCustomNetworkEvent(NetworkEventTarget.Owner, nameof(IncrementCount));
        //SendCustomNetworkEvent(NetworkEventTarget.All, "arrayFinish");
        //SendCustomNetworkEvent(NetworkEventTarget.All, "Spawn");
        //SendCustomNetworkEvent(NetworkEventTarget.All, "displayName");
        //SendCustomNetworkEvent(NetworkEventTarget.All, "AddUpScore");
        //SendCustomNetworkEvent(NetworkEventTarget.All, "DisableBuyButtons");
        timerUB.SendCustomNetworkEvent(NetworkEventTarget.All, "ActivateTimerMethod");
        SendCustomNetworkEvent(NetworkEventTarget.All, "EnableCollider");
    }
    public void Spawn()
    {
        if (isGeneral == true)
        {
            var card = cardGeneralArray[cardIntArray[count - 1]];
            var cardCopy = VRCInstantiate(card);

            currentCard = cardCopy;
            currentCardName = cardCopy.name;

            countOfCards = count;

            var cardPosition = spawnTarget.transform.position;
            var cardRotation = spawnTarget.transform.rotation;

            cardCopy.transform.SetPositionAndRotation(cardPosition, cardRotation);
            cardCopy.transform.localScale = cardScale;
        }
        else if (isWak == true)
        {
            var card = cardWakArray[cardIntArray[count - 1]];
            var cardCopy = VRCInstantiate(card);

            currentCard = cardCopy;
            currentCardName = cardCopy.name;

            countOfCards = count;

            var cardPosition = spawnTarget.transform.position;
            var cardRotation = spawnTarget.transform.rotation;

            cardCopy.transform.SetPositionAndRotation(cardPosition, cardRotation);
            cardCopy.transform.localScale = cardScale;
        }
        else
        {

        }
    }
    public void DestroyCard()
    {
        currentCard.SetActive(false);
    }
    public void IncrementCount()
    {
        // if player==owner 안전용
        //
        count += 1;
        RequestSerialization();
        OnCountChanged();
    }

    public override void OnDeserialization()
    {
        OnCountChanged();

    }

    // count 값이 변경됐을 때 감지해서 카드를 변경하는 함수
    public void OnCountChanged()
    {
        if(preCount != count)
        {
            //애니메이션 코드
            //딜레이
            OnAnimationFinished();
        }
        preCount = count;
        

    }
    public void OnAnimationFinished()
    {
        //변경된 인덱스에 맞는 물체를 가져오는 코드
        //오브젝트 소환
        //여기서 물건 이름 디스플레이하는 코드
        //DisableBuyButtons()
    }

    public void displayName()
    {
        if (currentCardName.Contains("(Clone)"))
        {
            t.text = currentCardName.Replace("(Clone)", "");
        }
        leftNumberOfCards.text = countOfCards.ToString() + " / 30";
    }
    public void arrayFinish()
    {

        if(count + 1 == 30)
        {
            isGeneral = false;
            isWak = false;
            nextButton.SetActive(false);
        }
    }
    public void BoxAnimation()
    {
        boxAnimator.SetTrigger("nextButton");
        boxSound.Play();
    }
    public void DisableBuyButtons()
    {
        for(int i = 0; i<4; i++)
        {
            buyButtonUdonBehaviour[i].SetProgramVariable("buyIsPressed", false);
            buyButtonUdonBehaviour[i].SendCustomNetworkEvent(NetworkEventTarget.All, "DisplayNone");
            questionButtonUB[i].SendCustomNetworkEvent(NetworkEventTarget.All, "TextToNull");
        }

    }
    public void DisableCollider()
    {
        bC.enabled = false;
        //SendCustomEventDelayedFrames("EnableCollider", 2);
    }
    public void EnableCollider()
    {
        bC.enabled = true;
        for (int i = 0; i < 4; i++)
        {
            questionBoxCollider[i].enabled = true;
        }
    }

    // 왁굳님께서 직접 의뢰하신 function
    // It's a function full of bugs just pehflekhaflk
 
    public void AddUpScore()
    {
        if (Networking.IsOwner(this.gameObject))
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkTestMethod_1");
            RequestSerialization();
        }
        else
        {
            //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            SendCustomNetworkEvent(NetworkEventTarget.All, "NetworkTestMethod_1");
            //Debug.Log("Owner of gameObject has changed"); 
        }
    }

    
    public void FindPlayer()
    {
        for(int i = 0; i < findTeamNumber.Length; i++)
        {
            var teamNumber = findTeamNumber[i].GetProgramVariable("teamNumber");
            switch (teamNumber)
            {
                case 1:
                    playerTeamNumber = 1;
                    break;
                case 2:
                    playerTeamNumber = 2;
                    break;
                case 3:
                    playerTeamNumber = 3;
                    break;
                case 4:
                    playerTeamNumber = 4;
                    break;
                default:
                    Debug.Log("can't find team");
                    break;

            }
            /*
            if(teamNumber.Equals(1))
            {
                playerTeamNumber = 1;
            }
            */
        }
    }

    public void FindPlayer2()
    {
        playerTag = Networking.LocalPlayer.GetPlayerTag("Team");
        Debug.Log(playerTag);
    }

    public void NetworkTestMethod_1()
    {
        int count = 0;
        for (int i = 0; i < buyButtonUdonBehaviour.Length; i++)
        {
            if (buyButtonUdonBehaviour[i].GetProgramVariable("buyIsPressed").Equals(true))
            {
                count++;
                Debug.Log("Buy button is counted");
            }
        }
        switch (playerTag)
        {
            case "1":
                counterUB.SetProgramVariable("count", count);
                counterUB.SendCustomNetworkEvent(NetworkEventTarget.Owner, "IncreaseTeamOne");
                break;
            case "2":
                counterUB.SetProgramVariable("count", count);
                counterUB.SendCustomNetworkEvent(NetworkEventTarget.Owner, "IncreaseTeamTwo");
                break;
            case "3":
                counterUB.SetProgramVariable("count", count);
                counterUB.SendCustomNetworkEvent(NetworkEventTarget.Owner, "IncreaseTeamThree");
                break;
            case "4":
                counterUB.SetProgramVariable("count", count);
                counterUB.SendCustomNetworkEvent(NetworkEventTarget.Owner, "IncreaseTeamFour");
                break;
        }
    }
}


