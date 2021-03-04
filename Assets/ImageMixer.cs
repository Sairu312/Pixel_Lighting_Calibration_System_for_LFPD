using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImageMixer : MonoBehaviour
{
    public bool MixStart = false;
    public bool OneMixStart = false;
    public Texture2D dotImage;
    public Texture2D stripeImgae;
    public int oneDispletImageNum;
    public Texture2D[] oneDispletImages = new Texture2D[483];
    public Texture2D[] exportImages = new Texture2D[483];
    public bool[] exportFlag = new bool[483];
    public string imagesName;
    public Vector2Int resolution;
    public int oneMixImageNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(setup())
        {
            RoadImaeges();
            //Debug.Log(dotImage.GetPixel(723, 720));
            //Debug.Log(oneDispletImages[241].GetPixel(720, 720));
            //Debug.Log(dotImage.GetPixel(723, 720) * oneDispletImages[241].GetPixel(720, 720));
            
        }else
        {
            Debug.Log("設定が間違ってます");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(MixStart)
        {
            CreateMultiplyImage();
            AssetDatabase.Refresh();
            //ExportMixImages();
            MixStart = false;
        }
        if(OneMixStart)
        {
            OneImageMix(oneMixImageNum);
            OneMixStart = false;
        }
    }

    bool setup()
    {
        oneDispletImages = new Texture2D[oneDispletImageNum];
        exportImages = new Texture2D[oneDispletImageNum];
   
        if (oneDispletImageNum != oneDispletImages.Length) return false;
        if (oneDispletImageNum != exportImages.Length) return false;
        exportFlag = new bool[oneDispletImageNum];
        return true;
    }

    void RoadImaeges()
    {
       
        for(int i = 0; i < oneDispletImageNum; i++)
        {
            string filePath = "OneDispletImages/" + i.ToString() + imagesName;
            //Debug.Log(filePath);
            oneDispletImages[i] = (Texture2D)Resources.Load(filePath);
        }     
    }

    void CreateMultiplyImage()
    {
        var tmpTexture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);
        Color[] dotTextureCol = new Color[resolution.x * resolution.y];
        Color[] oneDispletCol = new Color[resolution.x * resolution.y];
        Color[] tmpCol = new Color[resolution.x * resolution.y];
        Color[] stripeTextureCol = new Color[resolution.x * resolution.y];
        Vector3 sum = new Vector3(0f,0f,0f);
        dotTextureCol = dotImage.GetPixels(0, 0, resolution.x, resolution.y);
        stripeTextureCol = stripeImgae.GetPixels(0, 0, resolution.x, resolution.y);
        int exportNum = 0;
        for(int i = 0; i < oneDispletImages.Length; i++)
        {
            oneDispletCol = oneDispletImages[i].GetPixels(0, 0, resolution.x, resolution.y);
            for (int y = 0; y < resolution.y; y++)
            {
                for (int x = 0; x < resolution.x; x++)
                {
                    tmpCol[(y * resolution.x) + x] = dotTextureCol[(y * resolution.x) + x] * oneDispletCol[(y * resolution.x) + x];
                    sum = SumColors(sum, tmpCol[(y * resolution.x) + x]);
                    tmpTexture.SetPixel(x, y, tmpCol[(y * resolution.x) + x]);
                }
            }
            tmpTexture.Apply();
            if (sum.magnitude > 0f)
            {
                System.IO.File.WriteAllBytes(Application.dataPath + "/MixTileImages/" + exportNum + "_mix_tilie.png",
                                        tmpTexture.EncodeToPNG());

                for (int y = 0; y < resolution.y; y++)
                {
                    for (int x = 0; x < resolution.x; x++)
                    {
                        tmpCol[(y * resolution.x) + x] = stripeTextureCol[(y * resolution.x) + x] * oneDispletCol[(y * resolution.x) + x];
                        sum = SumColors(sum, tmpCol[(y * resolution.x) + x]);
                        tmpTexture.SetPixel(x, y, tmpCol[(y * resolution.x) + x]);
                    }
                }
                tmpTexture.Apply();
                System.IO.File.WriteAllBytes(Application.dataPath + "/MixTileStripe/" + exportNum + "_" + stripeImgae.name+"_" +dotImage.name+ "_Stripe_tilie.png",
                                        tmpTexture.EncodeToPNG());
                exportFlag[i] = true;
                //exportImages[i].Apply();
                exportNum += 1;
            }
            sum = Vector3.zero;
        }
    }

    Vector3 SumColors(Vector3 sum,Color col)
    {
        sum.x += col.r;
        sum.y += col.g;
        sum.z += col.b;
        return sum;
    }

    void ExportMixImages()
    {
        for (int i = 0; i < oneDispletImageNum; i++)
        {
            if(exportFlag[i])
            System.IO.File.WriteAllBytes(Application.dataPath + "/MixTileImages/" + i.ToString() + "_mix_tilie.png",
                                        exportImages[i].EncodeToPNG());
        }
        AssetDatabase.Refresh();
    }

    void OneImageMix(int select)
    {
        var tmpTexture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);
        Color[] dotTextureCol = new Color[resolution.x * resolution.y];
        Color[] oneDispletCol = new Color[resolution.x * resolution.y];
        Color[] tmpCol = new Color[resolution.x * resolution.y];
        dotTextureCol = dotImage.GetPixels(0, 0, resolution.x, resolution.y);
        oneDispletCol = oneDispletImages[select].GetPixels(0, 0, resolution.x, resolution.y);
        for(int y = 0; y < resolution.y; y++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                tmpCol[(y * resolution.x) + x] = dotTextureCol[(y * resolution.x) + x] * oneDispletCol[(y * resolution.x) + x];
                tmpTexture.SetPixel(x, y, tmpCol[(y * resolution.x) + x]);
            }
        }
        tmpTexture.Apply();
        System.IO.File.WriteAllBytes(Application.dataPath + "/MixTileImages/_mix_tilie.png",
                                        tmpTexture.EncodeToPNG());
        AssetDatabase.Refresh();
    }
}
