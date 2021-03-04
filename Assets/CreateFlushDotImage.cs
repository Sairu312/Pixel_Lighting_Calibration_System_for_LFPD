using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateFlushDotImage : MonoBehaviour
{
    public bool createImage = false;
    public int dotInterval = 8;
    public int dotSize = 4;
    public int xRes = 1440;
    public int yRes = 1440;
    private Texture2D FlushDotImage;
    private Vector2Int resolution;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if(createImage)
        {
            CreateDotTexture();
            FlushDotImage.Apply();

            System.IO.File.WriteAllBytes(Application.dataPath + "/DotTextures/" + dotSize.ToString() + "_" + dotInterval.ToString() + ".png",
                                        FlushDotImage.EncodeToPNG());
            AssetDatabase.Refresh();
            createImage = false;
        }
    }

    void SetUp()
    {
        resolution = new Vector2Int(xRes, yRes);
        FlushDotImage = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);
    }

    void CreateDotTexture()
    {
        BlackScreen();
        DrawDotSet();

    }

    private void BlackScreen()
    {
        for (int y = 0; y < FlushDotImage.height; y++)
        {
            for (int x = 0; x < FlushDotImage.width; x++)
            {
                FlushDotImage.SetPixel(x, y, Color.black);
            }
        }
    }

    private void DrawDotSet()
    {
        for(int y = 0; y < FlushDotImage.height; y++)
        {
            for(int x = 0; x < FlushDotImage.width; x++)
            {
                if(x%dotInterval == 0 && y%dotInterval == 0)
                {
                    DrawDot(x,y);
                }
            }
        }
    }

    private void DrawDot(int x, int y)
    {
        for(int i =0;i < dotSize; i++)
        {
            for (int j = 0; j < dotSize; j++)
            {
                FlushDotImage.SetPixel(x - (int)(dotSize / 2) + i, y - (int)(dotSize / 2) + j, Color.white);
            }
        }
    }
}
