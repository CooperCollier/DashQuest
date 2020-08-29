﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour {

	//--------------------------------------------------------------------------------

	Text text;

	public static int money;

	//--------------------------------------------------------------------------------

    void Start() {
        text = GetComponent<Text>();
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	money = Player.getMoney();
        text.text = "$" + money.ToString();
    }

    //--------------------------------------------------------------------------------
}
