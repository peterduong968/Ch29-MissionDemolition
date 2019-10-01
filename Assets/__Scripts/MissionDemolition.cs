using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// TODO: You must set the values for the enum
public enum GameMode
{
    idle,
    playing,
    levelEnd
}


// TODO: implement the MissionDemolition script
public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S;
    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles; //an array of the castles

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

	// Use this for initialization
	void Start ()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
	
	void StartLevel()
    {
        //Get rid of the old castle if one exists
        if(castle!= null)
        {
            Destroy(castle);
        }

        //destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        //NOT SURE IF THIS IS RIGHT. PG 714
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //Instantiate the new Castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Reset camera
        SwitchView("wShow Both");
        ProjectileLine.S.Clear();

        //reset goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        //show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {


    }

    void NextLevel()
    {

    }

    public void SwitchView(string eView = "")
    {

    }

    // Static method that allows code anywhere to increment shotsTaken
    public static void ShotFired()
    {

    }
}
