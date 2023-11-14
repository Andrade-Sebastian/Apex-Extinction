using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 40;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;


    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Displays")]
    //current stat displays
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit; //The time limit in seconds
    float stopwatchTime; //The current time elapsed since the stopwatch started
    public TMP_Text stopwatchDisplay;

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
    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        GameObject textObj =new GameObject("Damage FLoating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if(textFont) tmPro.font = textFont;
        rect.position = referenceCamera.WorldToScreenPoint(target.position);
        Destroy(textObj, duration);
        textObj.transform.SetParent(instance.damageTextCanvas.transform);
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while(t < duration)
        {
            yield return w;

            if (rect == null)
            {
                // Handle the case where the RectTransform is destroyed or null
                Destroy(textObj); // Clean up the temporary object
                yield break; // Exit the coroutine
            }

            t += Time.deltaTime;
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1-t / duration);
            yOffset += speed * Time.deltaTime;

            // Check if the target transform is still valid
            if (target == null)
            {
                // Handle the case where the target is destroyed or null
                Destroy(textObj); // Clean up the temporary object
                yield break; // Exit the coroutine
            }

            rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0,yOffset));
       }
    }
    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if(!instance.damageTextCanvas) return;

        if(!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(
            text, target, duration, speed
        ));
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
