using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gunimagestypes
{
    none,pistol,ar,sniper,rocket
};

public class UIManager : MonoBehaviour
{
    //Change Image when dead
    public GameObject p1, p2;
    public Image p1image, p2image;
    public Sprite p1dead, p1alive, p2dead, p2alive;

    //Health
    public Text p1healthtext, p2healthtext;
    public static float p1health, p2health;

    //Jet
    public Text p1jettext, p2jettext;
    public static float p1jet, p2jet;

    //Score
    public Text p1scoretext, p2scoretext;
    public static int p1score, p2score;

    //Ammo
    public Text p1ammotext, p2ammotext;
    public GameObject p1guncontainer, p2guncontainer;
    public Image p1gunimage, p2gunimage;
    public Sprite[] gunimages = new Sprite[6];
    public static gunimagestypes p1gunimagetype;
    public static gunimagestypes p2gunimagetype;
    public static float p1ammo, p2ammo;

    //Reach Score
    public GameObject reachScoreText;

    void Start()
    {
        p1score = 0;
        p2score = 0;
        StartCoroutine(ShowReachScore());
    }

    IEnumerator ShowReachScore()
    {
        reachScoreText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        reachScoreText.SetActive(false);
    }


    void Update()
    {
        //Change Image
        if (!p1.activeSelf)
        {
            p1image.sprite = p1dead;
        }
        else
        {
            p1image.sprite = p1alive;
        }
        if (!p2.activeSelf)
        {
            p2image.sprite = p2dead;
        }
        else
        {
            p2image.sprite = p2alive;
        }

        //Set Values
        //Health
        p1healthtext.text = p1health.ToString();
        p2healthtext.text = p2health.ToString();

        //Jet
        p1jettext.text = p1jet.ToString();
        p2jettext.text = p2jet.ToString();

        //Score
        p1scoretext.text = p1score.ToString();
        p2scoretext.text = p2score.ToString();

        //Ammo
        p1ammotext.text = p1ammo.ToString();
        p2ammotext.text = p2ammo.ToString();

        //Change p1 ammo image
        if(p1gunimagetype == gunimagestypes.none)
        {
            p1guncontainer.SetActive(false);
        }  
        if(p1gunimagetype == gunimagestypes.ar)
        {
            p1guncontainer.SetActive(true);
                p1ammotext.color = Color.yellow;    
            p1gunimage.sprite = gunimages[0];
        }
        if (p1gunimagetype == gunimagestypes.pistol)
        {
            p1guncontainer.SetActive(true);
            p1ammotext.color = Color.yellow;    
            p1gunimage.sprite = gunimages[1];
        }
        if (p1gunimagetype == gunimagestypes.sniper)
        {
            p1guncontainer.SetActive(true);
            p1ammotext.color = Color.yellow;
            p1gunimage.sprite = gunimages[2];
        }
        if (p1gunimagetype == gunimagestypes.rocket)
        {
            p1guncontainer.SetActive(true);
            p1ammotext.color = Color.green;
            p1gunimage.sprite = gunimages[3];
        }

        //Change p2 ammo image
        if (p2gunimagetype == gunimagestypes.none)
        {
            p2guncontainer.SetActive(false);
        }
        if (p2gunimagetype == gunimagestypes.ar)
        {
            p2guncontainer.SetActive(true);
            p2ammotext.color = Color.yellow;
            p2gunimage.sprite = gunimages[0];
        }
        if (p2gunimagetype == gunimagestypes.pistol)
        {
            p2guncontainer.SetActive(true);
            p2ammotext.color = Color.yellow;
            p2gunimage.sprite = gunimages[1];
        }
        if (p2gunimagetype == gunimagestypes.sniper)
        {
            p2guncontainer.SetActive(true);
            p2ammotext.color = Color.yellow;
            p2gunimage.sprite = gunimages[2];
        }
        if (p2gunimagetype == gunimagestypes.rocket)
        {
            p2guncontainer.SetActive(true);
            p2ammotext.color = Color.green;
            p2gunimage.sprite = gunimages[3];
        }
    }
}
