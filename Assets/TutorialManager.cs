using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI tutorialText;
    public bool pause;
    private int Messagecounter;
    //public tutorialPlayer player;
    public GameObject Arrow_left;
    public GameObject Arrow_right;
    public GameObject Arrow_down;
    public bool finished = false;

    void Start()
    {
        tutorialText.text = "Hello Sorcerer, ready for your training?";
        Messagecounter = 0;
        Arrow_down.SetActive(false);
        Arrow_left.SetActive(false);
        Arrow_right.SetActive(false);
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void Next()
    {
        Messagecounter += 1;
        UpdateMessage(Messagecounter);

    }

    public void SetText(string _text)
    {
        tutorialText.text = _text;
    }

    private void UpdateMessage(int counter)
    {
        switch (counter)
        {
            case 1:
                tutorialText.text = "First, let's go over the UI";
                break;
            case 2:
                tutorialText.text = "On the top left corner of the screen, is your health, level, experience progress and the enemies and coin count";
                Arrow_left.SetActive(true);
                break;
            case 3:
                Arrow_left.SetActive(false);
                tutorialText.text = "Place a finger on the screen the joystick will pop up and you can move the character around";
                break;
            case 4:
                tutorialText.text = "On the bottom right corner of the screen, is the skills and dash buttons, you know what dash is right? no? that's alright we find out later";
                Arrow_down.SetActive(true);
                break;
            case 5:
                tutorialText.text = "Oh, all the skills button are white because you haven't unlock any skills yet, keep killing the enemies and a skill panel will pop up and you get to pick what skills you want";
                break;
            case 6:
                Arrow_down.SetActive(false);
                tutorialText.text = "Finally, top right corner, this is a pause button, just in case you need to take a break";
                Arrow_right.SetActive(true);
                break;
            case 7:
                Arrow_right.SetActive(false);
                tutorialText.text = "Alright that is everything for the UI, let's do some practice! Move to the glowing area";
                //player.UnpauseCharacter();
                pause = false;
                this.gameObject.SetActive(false);
                break;
            default:
                //player.UnpauseCharacter();
                pause = false;
                Time.timeScale = 1;
                if (finished)
                {
                    SceneManager.LoadScene(1);
                }
                this.gameObject.SetActive(false);
                break;
        }
    }

    //Hello Sorcerer, ready for your training?
    //First，let's go over the UI.
    //On the top left corner of the screen, is your health, level, experience progress and the enemies and coin count;
    //On the bottom left corner of the screen, is your joysticks, you will use it to move the character, keep your finger on it!
    //On the bottom right corner of the screen, is the skills and dash buttons, you know what dash is right? no? that's alright we find out later!
    //Oh, all the skills button are white because you haven't unlock any yet, keep killing the enemies and a skill panel will pop up and you get to pick what skills you want.
    //Finally, top right corner, a pause button, just in case you need to take a break
    //Alright that is everything for the UI, let's do some practice! 
}
