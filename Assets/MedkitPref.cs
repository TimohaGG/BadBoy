using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitPref : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 50f;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
        transform.Rotate(new Vector3(90, 0));
        StartCoroutine(Rotate());
        
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            float newY = startPosition.y + Mathf.Sin(Time.time * 1f) * 0.5f;

            // Apply the new position
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.Heal();
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
