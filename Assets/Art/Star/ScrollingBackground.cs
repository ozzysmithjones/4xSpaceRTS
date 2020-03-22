using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float tileSize = 3f;
    public float paralax = 1f;
    public Vector2 additionalOffset = Vector2.zero;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        tileSize = transform.localScale.x / 5f;


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tiling; //= meshRenderer.material.mainTextureScale;
        tiling.x = Camera.main.orthographicSize * 5f / tileSize;
        tiling.y = Camera.main.orthographicSize * 5f / tileSize;
        meshRenderer.material.mainTextureScale = tiling;

        Vector2 offset; //= meshRenderer.material.mainTextureOffset;
        offset.x = (transform.position.x + additionalOffset.x) / transform.localScale.x / paralax * tiling.x;
        offset.y = (transform.position.y + additionalOffset.y) / transform.localScale.y / paralax * tiling.y;

        meshRenderer.material.mainTextureOffset = offset;
        // meshRenderer.material.color = Color.white * Mathf.Lerp(1.0f,0.0f, Camera.main.orthographicSize / 900.0f);


        transform.localScale = Vector3.one * Camera.main.orthographicSize * 5f;





    }
}
