using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyListener : MonoBehaviour {
    public PlayerAgent player;
    public BoostPowerup boost;
    public BreakerPowerup breaker;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            player.TurnEnd();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            boost.Activate();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            breaker.Activate();
        }

	}
}
