using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Different game states
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    //stores the current state of the game
    public GameState currentState;
    public GameState previousState;


    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Displays")]
    //current stat displays
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit; //The time limit in seconds
    float stopwatchTime; //The current time elapsed since the stopwatch started
    public Text stopwatchDisplay;

    public bool isGameOver = false;

    //Flag to check if the player is choosing their upgrades
    public bool choosingUpgrade;

    public GameObject playerObject;


    void Awake()
    {
        //Warning to check if there is a duplicate singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + "DELETED");
        }
        DisableScreens();
    }


    void Update()
    {
        //Behavior for each gamestate
        switch (currentState)
        {
            case GameState.Gameplay:
                //Gameplay state
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;
            case GameState.Paused:
                //Paused state code
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                //GameOver state code
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; //Stop the game
                    Debug.Log("Game over!");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if(!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; //Pause the game for now
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;    

            default:
                Debug.Log("Current state does not exist");
                break;
        }
    }


    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        { 
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; //Stop the gameplay
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused..");
        }

    }


    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resume game
            pauseScreen.SetActive(false);
            Debug.Log("Game resumed..");
        }
    }


    public void ChangeState(GameState newState)
    {
        currentState = newState;

    }


    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }
    }


    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }


    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveUI (List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen Weapons & Passive items data lists have different lengths");
            return;
        }

        //assign chosen weapons data
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            //check the sprite of the corresponding element
            if (chosenWeaponsData[i].sprite)
            {
                //enable the corresponding element
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                //disable the corresponding element
                chosenWeaponsUI[i].enabled = false;
            }
        }

        //assign chosen passive items data
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            //check the sprite of the corresponding element
            if (chosenPassiveItemsData[i].sprite)
            {
                //enable the corresponding element
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                //disable the corresponding element
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit)
        {
            // GameOver();
            playerObject.SendMessage("Kill");
        }
    }

    void UpdateStopwatchDisplay(){
        //Calculates the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60 );
        int seconds = Mathf.FloorToInt(stopwatchTime % 60 );


        //Update the stopwatch text to display the elapsed time
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; // Resumes the game
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
