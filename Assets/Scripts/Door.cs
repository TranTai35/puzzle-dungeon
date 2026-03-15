using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ParticleSystem openEffect;

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        StartCoroutine(OpenDoorAnimation());
    }

    IEnumerator OpenDoorAnimation()
    {
        if (openEffect != null)
            openEffect.Play();

        float duration = 0.5f;
        float timer = 0;

        Vector3 originalPos = transform.position;

        while (timer < duration)
        {
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.05f;

            timer += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}