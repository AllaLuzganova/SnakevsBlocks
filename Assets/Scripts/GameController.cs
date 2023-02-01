using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum GameState { MENU, GAME, GAMEOVER}
    public static GameState gameState;

    [Header("Managers")]

    public SnakeMovement SM;
    public BlocksManager BM;

    [Header("CanvasGroup")]

    public CanvasGroup MENU_CG;
    public CanvasGroup GAME_CG;
    public CanvasGroup GAMEOVER_CG;

    [Header("ScoreManagement")]

    public TMP_Text ScoreText;
    public TMP_Text MenuScoreText;
    public TMP_Text BestScoreText;
    public static int SCORE;
    public static int BESTSCORE;

    [Header("SomeBool")]
    bool speedAdded;

    void Start()
    {
        SetMenu();
        SCORE = 0;

        speedAdded= false;
        BESTSCORE = PlayerPrefs.GetInt("BESTSCORE");
    }

    void Update ()
    {
        ScoreText.text = SCORE + "";
        MenuScoreText.text = SCORE + "";

        if (SCORE > BESTSCORE)
            BESTSCORE = SCORE;

        BestScoreText.text = BESTSCORE + "";
            
        if (!speedAdded && SCORE > 150)
        {
            SM.speed++;
            speedAdded = true;
        }
    }

    public void SetMenu()
    {
        gameState = GameState.MENU;

        EnableCG (MENU_CG);
        DisableCG (GAME_CG);
        DisableCG (GAMEOVER_CG);
    }
    
    public void SetGameover()
    {
        gameState = GameState.GAMEOVER;

        EnableCG(MENU_CG);
        DisableCG(GAME_CG);
        DisableCG(GAMEOVER_CG);

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Box"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Bar"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("SimpleBox"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Snake"))
        {
            Destroy(g);
        }

        SM.SpawnBodyPart();

        BM.SetPreviousSnakePosAfterGameover(); //SetPreviousPosAfterGameover()

        speedAdded = false;  
        SM.speed = 3;

        PlayerPrefs.SetInt("BESTSCORE", BESTSCORE);
        BM.SimpleBoxPositions.Clear();
    }

    public void EnableCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public void DisableCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }    
}
