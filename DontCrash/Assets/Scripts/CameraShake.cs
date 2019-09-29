using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magnitude){
        Vector3 originalPos = new Vector3(9f, 9f, -8.5f);
        float elapsed = 0.0f;
        
        while (elapsed < duration){
            float x = Random.Range(-.1f, .1f) * magnitude;
            float z = Random.Range(-.1f, .1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y, originalPos.z + z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
