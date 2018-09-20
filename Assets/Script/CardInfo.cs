using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour {
    public Texture2D[] textures ; //声明一个数组型的图片库；
    private float i = 0; //声明i为浮点数0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeLanguage(string language)
    {
        Debug.Log("changeLanguage, language=" + language);
        if (language == null) return;
        if (language.StartsWith("zh"))
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = textures[0];
        } else
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = textures[1];
        }
    }

    public void updateTexture(int index)
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = textures[index];
    }
}
