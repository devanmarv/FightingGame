using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Color[] playerColors;
    public List<PlayerController> players = new List<PlayerController>();
    public Transform[] spawnPoints;

    [Header("Componets")]
    public GameObject deathEffectPrefab;

    public static PlayerManager instance;
    public GameObject playerContainerPrefab;
    public Transform playerContainerParent;

    public TextMeshProUGUI timeText;
    public float roundtime;
    public bool timer_is_started = false;


    private void Awake()
    {
        instance = this;
        timer_is_started = false;
        timeText.text = roundtime.ToString();
    }


    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // set player color
        playerInput.GetComponentInChildren<SpriteRenderer>().color = playerColors[players.Count];
        players.Add(playerInput.GetComponent<PlayerController>());
        playerInput.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        // create a UI for player
        playerContainerUi containerUI = Instantiate(playerContainerPrefab, playerContainerParent).GetComponent<playerContainerUi>();
        playerInput.GetComponent<PlayerController>().containerUI = containerUI;
        containerUI.Initialize(playerInput.GetComponentInChildren<SpriteRenderer>().color);
        timer_is_started = true;
    }

    public void OnPlayerDeath(PlayerController player, PlayerController attacker)
    {
        // spawn death effect
        Destroy(Instantiate(deathEffectPrefab,player.transform.position,Quaternion.identity),1.5f);

        // increase score for attacker
     
        if (attacker != null)
        {
            attacker.score++;
            //update score text in hud
            attacker.containerUI.updateScoreText(attacker.score);
        }
        else
        {
            player.score--;
            if (player.score < 0){
                player.score = 0;
            }
            //update score text in hud
            player.containerUI.updateScoreText(player.score);
        }
        

        // respawn player
        player.spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].position);
        
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer_is_started)
        {
            roundtime -= Time.deltaTime;
            int timeupdate = (int)roundtime;
            timeText.text = timeupdate.ToString();
        }
        if (roundtime < 0.0f)
        {
            SceneManager.LoadScene("levelselect");
        }
    }
}
