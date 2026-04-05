using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ParticleSystem openEffect;

 

    public void OpenDoor()
    {

        AudioController.Instance.PlaySoundDoor();

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

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}