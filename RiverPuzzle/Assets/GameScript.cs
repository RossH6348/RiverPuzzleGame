using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{


    //A bunch of variables we are going to need.
    //First one being a dictionary of what equals what.
    Dictionary<string, Button> Objects = new Dictionary<string, Button>();

    //Three lists to keep track where objects are currently is.
    List<string> LeftSide = new List<string>();
    List<string> onBoat = new List<string>();
    List<string> RightSide = new List<string>();

    //This bool is to determine which side the boat is on.
    bool isBoatOnLeft = true;

    //Bunch of public text objects to display as wins or loses.
    public Text Win;
    public Text LoseA;
    public Text LoseB;
    public Text Hint;


    // Start is called before the first frame update
    void Start()
    {

        //Get all objects.
        Objects.Add("Man", GameObject.Find("Man").GetComponent<Button>());
        Objects.Add("Wolf", GameObject.Find("Wolf").GetComponent<Button>());
        Objects.Add("Goat", GameObject.Find("Goat").GetComponent<Button>());
        Objects.Add("Carrots", GameObject.Find("Carrots").GetComponent<Button>());
        Objects.Add("Boat", GameObject.Find("Boat").GetComponent<Button>());

        //Initialize all lists and positions back to the start so it is a new fresh game.
        isBoatOnLeft = true;
        LeftSide.Add("Man");
        LeftSide.Add("Wolf");
        LeftSide.Add("Goat");
        LeftSide.Add("Carrots");

        //Disable all text objects from appearing.
        Win.enabled = LoseA.enabled = LoseB.enabled = Hint.enabled = false;

        Invoke("ShowHint", 120.0f); //Give a player a hint, after 2 minutes.
    }

    // Update is called once per frame
    void Update()
    {
        //First let do all the checking nonsense.
        if (isBoatOnLeft)
        {
            //Check the status of the right island.
            if (!RightSide.Contains("Man"))
            {
                //Since goat is involved in both scenarios, we can just simplify it a bit more.
                if (RightSide.Contains("Goat"))
                {
                    if (RightSide.Contains("Wolf"))
                    {
                        //WOLF EATS THE GOAT!
                        LoseB.enabled = true;
                        Invoke("ResetGame", 5.0f);
                    }
                    else if (RightSide.Contains("Carrots"))
                    {
                        //GOAT EATS THE CARROT!
                        LoseA.enabled = true;
                        Invoke("ResetGame", 5.0f);
                    }
                }
            }
        }
        else
        {
            //Check the status of the left island.
            if (!LeftSide.Contains("Man"))
            {
                //Since goat is involved in both scenarios, we can just simplify it a bit more.
                if (LeftSide.Contains("Goat"))
                {
                    if (LeftSide.Contains("Wolf"))
                    {
                        //WOLF EATS THE GOAT!
                        LoseB.enabled = true;
                        Invoke("ResetGame", 5.0f);
                    }
                    else if (LeftSide.Contains("Carrots"))
                    {
                        //GOAT EATS THE CARROT!
                        LoseA.enabled = true;
                        Invoke("ResetGame", 5.0f);
                    }
                }
            }
        }


        if(RightSide.Contains("Man") && RightSide.Contains("Wolf") && RightSide.Contains("Goat") && RightSide.Contains("Carrots"))
        {
            //The player have beaten the puzzle.
            Win.enabled = true;
            Invoke("ResetGame", 5.0f);
        }

        //Okay nothing have occurred, so let start auto-positioning.
        float XPos = -640.0f;
        float YPos = -218.0f;
        foreach(string obj in LeftSide)
        {
            Button entity = Objects[obj];
            entity.transform.localPosition = new Vector3(XPos, YPos, entity.transform.localPosition.z);
            XPos += entity.GetComponent<RectTransform>().rect.width;
        }

        XPos = Objects["Boat"].transform.localPosition.x + 64.0f;
        YPos = Objects["Boat"].transform.localPosition.y + 84.0f;
        foreach (string obj in onBoat)
        {
            Button entity = Objects[obj];
            entity.transform.localPosition = new Vector3(XPos, YPos, entity.transform.localPosition.z);
            XPos += entity.GetComponent<RectTransform>().rect.width;
        }

        XPos = 184.0f;
        YPos = -218.0f;
        foreach (string obj in RightSide)
        {
            Button entity = Objects[obj];
            entity.transform.localPosition = new Vector3(XPos, YPos, entity.transform.localPosition.z);
            XPos += entity.GetComponent<RectTransform>().rect.width;
        }

    }



    //This function is for transferring one object from one list to another.
    void TransferObject(string obj,List<string> ListA,List<string> ListB)
    {
        ListA.Remove(obj);
        ListB.Add(obj);
    }

    //Function to move the boat.
    public void MoveBoat()
    {

        if (!onBoat.Contains("Man"))
            return; //The boat cannot move without the man operating it.

        if (isBoatOnLeft)
        {
            //Transfer the boat to the right island of the river.
            Objects["Boat"].transform.localPosition += new Vector3(720.0f,0.0f,0.0f);
        }
        else
        {
            //Vice versa...
            Objects["Boat"].transform.localPosition += new Vector3(-720.0f, 0.0f, 0.0f);
        }

        isBoatOnLeft = !isBoatOnLeft;

    }

    //This function is responsible for transfering objects onto boat and islands.
    public void onObjectClick(GameObject obj)
    {
        string ObjectName = obj.name;
        if (onBoat.Contains(ObjectName))
        {
            if (isBoatOnLeft)
                TransferObject(ObjectName, onBoat, LeftSide);
            else
                TransferObject(ObjectName, onBoat, RightSide);
        }
        else if(onBoat.Count < 2) //Make sure if it is less than 2 entities on the boat.
        {
            if (isBoatOnLeft)
            {
                if (LeftSide.Contains(ObjectName))
                    TransferObject(ObjectName, LeftSide, onBoat);
            }
            else
            {
                if (RightSide.Contains(ObjectName))
                    TransferObject(ObjectName, RightSide, onBoat);
            }
        }
    }

    //This function is for showing hint.
    void ShowHint()
    {
        Hint.enabled = true;
    }

    //This function is for resetting the game.
    public void ResetGame()
    {

        //Reset the boat.
        Vector3 pos = Objects["Boat"].transform.localPosition;
        Objects["Boat"].transform.localPosition = new Vector3(-560.0f, pos.y, pos.z);
        isBoatOnLeft = true;

        //Reset all the lists.
        while (onBoat.Count > 0)
            onBoat.RemoveAt(0);

        while (RightSide.Count > 0)
            RightSide.RemoveAt(0);

        while (LeftSide.Count > 0)
            LeftSide.RemoveAt(0);

        LeftSide.Add("Man");
        LeftSide.Add("Wolf");
        LeftSide.Add("Goat");
        LeftSide.Add("Carrots");

        //Disable all text objects from appearing.
        Win.enabled = LoseA.enabled = LoseB.enabled = false;
    }
}