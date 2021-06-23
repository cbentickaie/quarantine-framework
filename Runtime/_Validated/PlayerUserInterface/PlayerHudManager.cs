using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerHudActions { ShowMessage, ShowTutorial, Showprompt, ShowObjective, ShowObjectiveComplete };
public class PlayerHudManager : MonoBehaviour {
    
    //Variables
    Animator HudAnimator;
    public Text PlayerObjectiveText;
    public Text PlayerMessageText;
    public Text PlayerPromptText;
    public Image crosshair;
    public float messageTime = 2.0f;
    public bool showCrosshair = true;
    [SerializeField] bool showCrosshairOnHover = true;
    public GameObject statspanel;
    public GameObject pauseMenu;
    public List<Slider> StatSliders;
    public Color crossHairColor;
    [SerializeField]
    DamageHandler playerDH;
    public Image DamageOverlayImage;
    public Sprite[] DamageTextures;
    //Singleton Setup
    public static PlayerHudManager instance = null; //Static instance of PlayerHudManager which allows it to be accessed by any other script.
                                                    //Awake is always called before any Start functions
    [ExecuteInEditMode]
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        HudAnimator = gameObject.GetComponent<Animator>();
        
    }


    void Start()
    {

        TogglePlayerStatsPanel();
        crossHairColor = crosshair.color;
        if (!showCrosshair)
        {
            crosshair.gameObject.SetActive(false);
        }
        if (playerDH = (GameObject.FindGameObjectWithTag("Player").GetComponent<DamageHandler>()))
        {
            UpdateStatsPanel();
        }
        //crossHairColor = crosshair.color;
    }

    public void DisplayPlayerObjective(string inMessage, float duration = 2.0f)
    {
        PlayerObjectiveText.text = inMessage;
        HudAnimator.Play("Anim_UI_ObjectiveAppear", 0);
        StartCoroutine(HidePlayerObjective(duration));
    }

    public void DisplayPlayerMessage(string inMessage, float duration = 2.0f)
    {
        PlayerMessageText.text = inMessage;
        HudAnimator.Play("Anim_UI_MessageAppear", 0);
        StartCoroutine(HidePlayerMessage(duration));
    }

    public void DisplayPlayerPrompt(string inMessage, bool holdPrompt, float duration = 2.0f)
    {
        PlayerPromptText.text = inMessage;
        HudAnimator.Play("Anim_UI_PromptAppear", 0);
        if (!holdPrompt)
        {
            StartCoroutine(HidePlayerPrompt(duration));
        }        
    }

    public void TogglePlayerStatsPanel() 
    {
        if (statspanel) 
        {
            statspanel.SetActive(!statspanel.activeSelf);
            UpdateStatsPanel();
        }   
    }

    public void UpdateStatsPanel() 
    {
        if(playerDH)
        {
            StatSliders[0].value = playerDH.HealthRatio;
        }
    }

    public void DisplayObjectiveComplete()
    {        
        DisplayPlayerObjective("Objective Complete");
    }

    public void ShowDamageFlash(float damageRatio = 1.0f) 
    {
        if (damageRatio > 0.5f)
        {
            DamageOverlayImage.sprite = DamageTextures[1];
        }
        else 
        {
            DamageOverlayImage.sprite = DamageTextures[0];
        }
        HudAnimator.Play("Anim_UI_DamageFlash", 0, 0.0f);
        
    }

    public void DoScreenFade(bool FadeIn)
    {
        if (FadeIn)
        {
            HudAnimator.Play("Anim_UI_FadeFromBlack", 0);
        }
        else 
        {
            HudAnimator.Play("Anim_UI_FadeToBlack", 0);
        }
        print("DID FADE");
    }

    //Coroutines for hiding UI elements when duration complete
    public IEnumerator HidePlayerObjective(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        HudAnimator.Play("Anim_UI_ObjectiveDisappear", 0);
    }

    public IEnumerator HidePlayerMessage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        HudAnimator.Play("Anim_UI_MessageDisAppear", 0);
    }

    public IEnumerator HidePlayerPrompt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        HudAnimator.Play("Anim_UI_PromptDisAppear", 0);
    }
    public Color idleColor;
    public Color HoverColor;
    public bool isHovering = false;
    private void Update()
    {
        if (Input.GetButtonDown("ShowStatsPanel")) 
        {
            print("Stat button pressed!");
            TogglePlayerStatsPanel();
        }
        if (statspanel.activeSelf) 
        {
            UpdateStatsPanel();
        }
        if (showCrosshair) 
        {
           //crosshair.color = idleColor;
        }
    }

    public void toggleHover(bool newHovering) 
    {
        isHovering = newHovering;
        if (isHovering)
        {            
            crosshair.color = HoverColor;
            //print("DIDHOVER");
            if (!showCrosshair && showCrosshairOnHover) 
            {
                //show crosshair
                crosshair.gameObject.SetActive(true);
            }
        }
        else if(!isHovering) 
        {
            crosshair.color = idleColor;
            //print("STOPHOVER");
            if (!showCrosshair && showCrosshairOnHover)
            {
                //hide crosshair
                crosshair.gameObject.SetActive(false);
            }
        }
    }
    
    public void TogglePauseMenu(bool showMenu) 
    {
        pauseMenu.SetActive(showMenu);
    }
}
