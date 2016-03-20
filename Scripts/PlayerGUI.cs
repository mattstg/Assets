using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
    PlayerController player;
    public Slider energyBar;
	// Use this for initialization
	void Start () {
        player = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        energyBar.value = (player.playerEnergy/GV.PLAYER_MAX_HEALTH)*100;
	}
}

