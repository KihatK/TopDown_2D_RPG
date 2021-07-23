using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public TypeEffect typeEffect;
    public Animator talkPanel;
    public Image portraitImg;
    public Animator portraitAnim;
    public GameObject menuSet;
    public GameObject player;
    public Text questText;
    public Text nameText;
    public bool isAction;
    public int talkIndex;

    GameObject scanObject;
    Sprite prevPortrait;

    private void Start() {
        GameLoad();
        questText.text = questManager.CheckQuest();
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            SubMenuActive();
        }
    }

    public void SubMenuActive() {
        if (!isAction) {
            if (menuSet.activeSelf) {
                menuSet.SetActive(false);
            }
            else {
                menuSet.SetActive(true);
            }
        }
    }

    public void Action(GameObject scanObj) {
        scanObject = scanObj;
        ObjData objData = scanObj.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc, objData.npcName);

        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc, string npcName) {
        if (typeEffect.isAnim) {
            typeEffect.SetMsg("");
            return;
        }
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        if (talkData == null) {
            isAction = false;
            talkIndex = 0;
            //questActionIndex + 1
            questText.text = questManager.CheckQuest(id);
            return;
        }

        //Name Text
        nameText.text = npcName;

        if (isNpc) {
            typeEffect.SetMsg(talkData.Split(':')[0]);

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
            //Portrait Animation
            if (prevPortrait != portraitImg.sprite) {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }
        }
        else {
            typeEffect.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);

        }

        isAction = true;
        talkIndex++;
    }

    public void GameSave() {
        //player.x, player.y
        //questId
        //questActionIndex
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad() {
        if (!PlayerPrefs.HasKey("PlayerX")) {
            return;
        }
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), 0);
        questManager.questId = PlayerPrefs.GetInt("QuestId");
        questManager.questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        questManager.ControlObject();
    }

    public void GameExit() {
        Application.Quit();
    }
}
