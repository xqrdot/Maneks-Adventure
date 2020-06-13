using UnityEngine;

public class PickupAbility : MonoBehaviour
{
	[SerializeField] AudioClip sfx_pickup = null;
	Vector3 _startPos;
    AudioSource source;

    private void Start()
    {
        source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
		_startPos = transform.position;
    }

	private void Update() {
		this.transform.position = _startPos + new Vector3(0, Mathf.Sin(Time.time * 3) / 2);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		ManagerEvents.current.TeleportActivate(true);
		
		if (sfx_pickup != null) source.PlayOneShot(sfx_pickup);
		print("Activated Teleport Ability for a player");

		Destroy(gameObject);
	}
}