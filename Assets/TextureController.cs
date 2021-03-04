using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour
{
    public float PixelFlushInterbal = 1.0f;
    public Texture2D LFPtexture;
    public bool goFlushFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        LFPtexture = new Texture2D(1440, 1440, TextureFormat.RGBA32, false);
        WhiteScreen();
        GetComponent<Renderer>().material.mainTexture = LFPtexture;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if(goFlushFlag)
        { 
            StartCoroutine("PixelFlush");
            goFlushFlag = false;
        }
        LFPtexture.Apply();
    }

    IEnumerator PixelFlush()
    {
        BlackScreen();
        for (int y = 0; y < LFPtexture.height; y++)
        {
            for (int x = 0; x < LFPtexture.width; x++)
            {
                //BlackScreen();
                LFPtexture.SetPixel(x, y, Color.white);
                Debug.Log("chenge!");
                yield return new WaitForSeconds(PixelFlushInterbal);
            }
        }
        goFlushFlag = false;
    }

    private void BlackScreen()
    {
        for (int y = 0; y < LFPtexture.height; y++)
        {
            for (int x = 0; x < LFPtexture.width; x++)
            {
                LFPtexture.SetPixel(x, y, Color.black);
            }
        }
        
    }

    private void WhiteScreen()
    {
        for (int y = 0; y < LFPtexture.height; y++)
        {
            for (int x = 0; x < LFPtexture.width; x++)
            {
                LFPtexture.SetPixel(x, y, Color.white);
            }
        }
       
    }

    private void CheckInput()
    {
        if(Input.GetKey(KeyCode.Space) && !goFlushFlag)
        {
            goFlushFlag = true;
        }
    }

}
