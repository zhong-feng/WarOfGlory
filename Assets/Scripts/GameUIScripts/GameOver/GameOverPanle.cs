using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverPanle : MonoBehaviour 
{
    Image WinImage;
    Image LossImage;
    Button SubmitButton;

    public Transform MonsterParent;
    public Transform BirthPlaceParent;

	// Use this for initialization
	void Start () 
	{
        SubmitButton = this.transform.Find("SubmitButton").GetComponent<Button>();
        SubmitButton.onClick.AddListener(OnBackLoginScene);

        WinImage = this.transform.Find("WinImage").GetComponent<Image>();
        LossImage = this.transform.Find("LossImage").GetComponent<Image>();
	}

    void OnBackLoginScene()
    {
        //确认下一个场景
        PlayerDefine.Instance.NextScene = SceneType.ST_MainUIScene;

        //从异步加载场景切换到目标场景
        SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Single);
    }

    public void GameOver(bool isWin)
    {
        if(isWin)
        {
            LossImage.gameObject.SetActive(false);
        }
        else
        {
            WinImage.gameObject.SetActive(false);
        }

        //var monsterArray = MonsterParent.GetComponentsInChildren<MonsterAI>();
        //for(int i=0;i<monsterArray.Length;i++)
        //{
        //    monsterArray[i].enabled = false;
        //    monsterArray[i].GetComponent<NavMeshAgent>().enabled = false;
        //}

        //var monsterBirthPlaceArray = BirthPlaceParent.GetComponentsInChildren<MonsterBirthplace>();
        //for(int j=0;j<monsterBirthPlaceArray.Length;j++)
        //{
        //    monsterBirthPlaceArray[j].gameObject.SetActive(false);
        //}

        //var gameConstroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //gameConstroller.gameObject.SetActive(false);

        this.transform.SetAsLastSibling();

        this.transform.DOMove(Vector3.zero, 0.2f);
    }
	
}
