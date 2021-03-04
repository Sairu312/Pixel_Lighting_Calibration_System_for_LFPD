using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateStripe : MonoBehaviour
{
    public int f;
    public bool create = false;
    public bool yAxis = false;

    public int xRes = 1440;
    public int yRes = 1440;
    private Texture2D stripeImage;
    private Vector2Int resolution;
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if(create)
        {
            CreateStripeImage();
            create = false;
        }
    }

    void SetUp()
    {
        resolution = new Vector2Int(xRes, yRes);
        stripeImage = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);
    }

    void CreateStripeImage()
    {
        DrawStripeX();
        stripeImage.Apply();
        if(yAxis)
            System.IO.File.WriteAllBytes(Application.dataPath + "/StripeImages/Y_" + f.ToString() + ".png",
                                       stripeImage.EncodeToPNG());
        else
        System.IO.File.WriteAllBytes(Application.dataPath + "/StripeImages/X_" + f.ToString()+ ".png",
                                       stripeImage.EncodeToPNG());
        AssetDatabase.Refresh();
    }

    private void BlackScreen()
    {
        for (int y = 0; y < stripeImage.height; y++)
        {
            for (int x = 0; x < stripeImage.width; x++)
            {
                stripeImage.SetPixel(x, y, Color.black);
            }
        }
    }

    private void DrawStripeX()
    {
        for (int y = 0; y < stripeImage.height; y++)
        {
            for (int x = 0; x < stripeImage.width; x++)
            {
                if (yAxis)
                {
                    if ((int)(y / f - (f/2)) % 2== 0) stripeImage.SetPixel(x, y, Color.black);
                    else stripeImage.SetPixel(x, y, Color.white);
                }
                else
                {
                    if ((int)(x / f - (f/2)) % 2 == 0) stripeImage.SetPixel(x, y, Color.black);
                    else stripeImage.SetPixel(x, y, Color.white);
                }
            }
        }
    }
}
