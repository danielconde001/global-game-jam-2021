using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject player;
    public GameObject Player { get { return player; } }

    private FPSControllerLPFP.FpsControllerLPFP fpsController;
    public FPSControllerLPFP.FpsControllerLPFP FpsController { get { return fpsController; } }

    private HandgunScriptLPFP handgun;
    public HandgunScriptLPFP Handgun { get { return handgun; } }
    
    private Canvas playerCanvas;
    public Canvas PlayerCanvas { get { return playerCanvas; } }

    private PlayerInteract playerInteract;
    public PlayerInteract PlayerInteract { get { return playerInteract; } }

    private AudioSource playerAudioSource;
    public AudioSource PlayerAudioSource { get { return playerAudioSource; } }

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        fpsController = player.GetComponent<FPSControllerLPFP.FpsControllerLPFP>();
        handgun = player.GetComponentInChildren<HandgunScriptLPFP>();
        playerCanvas = player.GetComponentInChildren<Canvas>();
        playerInteract = player.GetComponent<PlayerInteract>();
        playerAudioSource = player.GetComponent<AudioSource>();
    }
}
