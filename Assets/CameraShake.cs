using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
 public class CameraShake : MonoBehaviour
 {
     // How long the shake lasts
     public float duration = 0.5f;
     // How strong the shake is
     public float magnitude = 0.2f;
 
     [ContextMenu("Shake Camera")]
     public void TriggerShake()
     {
         StopAllCoroutines();
         StartCoroutine(Shake());
     }
 
     IEnumerator Shake()
     {
         Vector3 originalPos = transform.localPosition;
         float elapsed = 0.0f;
 
         while (elapsed < duration)
         {
             // Generate random offset
             float x = Random.Range(-1f, 1f) * magnitude;
             float y = Random.Range(-1f, 1f) * magnitude;
 
             transform.localPosition = new Vector3(x, y, originalPos.z);
 
             elapsed += Time.deltaTime;
 
             // Wait until next frame
             yield return null;
         }
 
         transform.localPosition = originalPos;
     }
 }
