using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlanetTexture : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Texture2D texture2D;

    public bool isGasGiant = false;
    public Sprite GasGiantSprite;

    public Color seaColor;
    public Color landColor;

    public float zoom = 20f;
    public Vector2 seed;

    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //if there is no sprite Renderer, then this script is disabled.
        if(spriteRenderer == null)
        {
            this.enabled = false;
            return;
        }

       

        texture2D = new Texture2D(32,32,TextureFormat.RGBA32,false);
        Graphics.CopyTexture(spriteRenderer.sprite.texture, texture2D);

        texture2D.filterMode = FilterMode.Point;

        //if the texture on the sprite render cannot be edited, then this script is disabled too.
        if (!texture2D.isReadable)
        {
            print("the texture "+texture2D.name+" is not readable");
            this.enabled = false;
            return;
        }
        //otherwise, the planets texture is changed depending upon the seed and values set on the script. 
        Generate();

        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            //Generate();
       //}
    }

    void Generate()
    {
        if (isGasGiant)
        {
            spriteRenderer.sprite = GasGiantSprite;
            spriteRenderer.color = landColor;
            return;
        }



        spriteRenderer.material.mainTexture = texture2D;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", texture2D);
        
        spriteRenderer.SetPropertyBlock(block);
        for (int x= 0; x < 32; x++)
        {
            for(int y = 0;y < 32; y++)
            {
                ColorPixel(x, y);
            }
        }
        
        texture2D.Apply();
    }

    void ColorPixel(int x, int y)
    {
        if (texture2D.GetPixel(x, y).a > 0f)
        {
            float xf = (float)x / 32f + (float)seed.x;
            float yf = (float)y / 32f + (float)seed.y;


            Color color = Color.magenta;

            float perlin = Mathf.PerlinNoise(xf * zoom, yf * zoom);

            if (perlin > 0.5f)
            {
               
                color = landColor;
            }
            else
            {
                
                color = seaColor;
            }

            texture2D.SetPixel(x, y, color);
        }

    }

    public void SetValues(Color land, Color sea, Vector2 noiseSeed,float zoomAmount)
    {
        landColor = land;
        seaColor = sea;
        seed = noiseSeed;
        zoom = zoomAmount;

    }
}
