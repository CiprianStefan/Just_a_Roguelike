using UnityEngine;

public class DisplayDamageUIInstance : MonoBehaviour
{
    public Vector3 spawnPosition;

    protected void Update() {
        ((RectTransform)transform).localScale -= new Vector3(Time.deltaTime*2,Time.deltaTime*2,Time.deltaTime*2); 
        ((RectTransform)transform).position = spawnPosition;
        if(((RectTransform)transform).localScale.x <= 0)
            gameObject.SetActive(false);
    }
}
