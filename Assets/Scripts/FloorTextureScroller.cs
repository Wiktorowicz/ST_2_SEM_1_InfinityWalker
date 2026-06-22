using UnityEngine;

public class FloorTextureScroller : MonoBehaviour {
    [SerializeField] private float textureSpeedMultiplier = 0.1f;

    private Material floorMaterial;
    private Vector2 textureOffset;

    private void Awake() {
        floorMaterial = GetComponent<Renderer>().material;
    }

    private void Update() {
        float scrollSpeed = GameManager.Instance.WorldSpeed * textureSpeedMultiplier;


        textureOffset.y -= scrollSpeed * Time.deltaTime;

        floorMaterial.SetTextureOffset("_BaseMap", textureOffset);
        floorMaterial.SetTextureOffset("_BumpMap", textureOffset);
    }
}