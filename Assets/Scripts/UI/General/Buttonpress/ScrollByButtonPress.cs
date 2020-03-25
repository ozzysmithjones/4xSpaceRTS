using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollByButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform content;
    public float scrollDirection = 1.0f;
    private float scrollSpeed = 400.0f;
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Scroll());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private IEnumerator Scroll()
    {
        while (true)
        {
            var pos = content.position;
            pos.x += scrollSpeed * Time.deltaTime * scrollDirection;
            content.position = pos;
            yield return new WaitForEndOfFrame();
        }
    }

   
}
