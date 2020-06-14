#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Transform respawnPosition = null;

    private void Start() {
        if (respawnPosition == null)
        {
            respawnPosition = this.gameObject.transform;
        }
    }

	// Start is called before the first frame update
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
        {
            StaticStorage.instance.SetRespawnPosition(respawnPosition);
            this.gameObject.SetActive(false);
        }
	}
}
