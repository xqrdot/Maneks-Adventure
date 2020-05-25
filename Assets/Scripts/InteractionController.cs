using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
  [Header("Teleportation")]
  [SerializeField] private float teleportDelayTime = 0.125f;
  [SerializeField] private float teleportChargeTime = 2.5f;
  [SerializeField] private float teleportMaxDistance = 13f;
  [SerializeField] GameObject teleportEndpoint;
  [SerializeField] LineRenderer line;
  [SerializeField] public Color colorTeleportClear;
  [SerializeField] public Color colorTeleportBlocked;

  [Header("Combat")]
  [SerializeField] GameObject throwable;
  [SerializeField] float throwForce = 45f;
  [SerializeField] float throwRate = 0.15f;
  [SerializeField] float meleeRate = 0.3f;
  [SerializeField] float meleeRange = 3.5f;
  [SerializeField] float meleeDamage = 4f;
  [SerializeField] float rangeDamage = 2f;

  [SerializeField] AudioClip sfx_throw = null;
  [SerializeField] AudioClip sfx_melee = null;


  [Header("SFX")]
  [SerializeField] AudioClip sfx_teleportIn;
  [SerializeField] AudioClip sfx_teleportOut;


  Vector3 teleportTarget;

  GameObject player;
  AudioSource source; 

  bool canTeleport = true;
  bool isAiming = false;
  bool canActivateInteractable = false;
  bool interactableRequest = false;
  float teleportTime;
  float lineAlpha = 0;
  float nextAttackThrowTime = 0f;
  float nextAttackMeleeTime = 0f;

  ManagerUI managerUI;
  Camera mainCamera;


  // Start is called before the first frame update
  void Start()
  {
    teleportTime = teleportChargeTime;

    player = StaticStorage.instance.Player;
    source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
    StaticStorage.instance.teleportMaxCooldown = teleportChargeTime;

    managerUI = StaticStorage.instance.GameManager.GetComponent<ManagerUI>();
    mainCamera = Camera.main;
    

    teleportEndpoint.SetActive(false);
    //line.enabled = false;
  }

  private void FixedUpdate()
  {
    CheckInteractables();
  }
  // Update is called once per frame
  void Update()
  {
    StaticStorage.instance.teleportCurrentCooldown = teleportTime;

    if (!canTeleport) {
      teleportTime += Time.deltaTime;

      if (teleportTime >= teleportChargeTime) {
        canTeleport = true;
        teleportTime = teleportChargeTime;
      }
    }


    if (isAiming) {
      lineAlpha += Time.deltaTime;
      lineAlpha = Mathf.Clamp01(lineAlpha);
    }
    else {
      lineAlpha -= Time.deltaTime * 3;
      lineAlpha = Mathf.Clamp01(lineAlpha);
    }

    // Cache mouse pos for this frame 
    var mousePos = Input.mousePosition;
    var mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
    var playerWorldPos = player.transform.position;

    teleportEndpoint.transform.position = mouseWorldPos;

    line.SetPosition(0, playerWorldPos);
    line.SetPosition(1, mouseWorldPos);

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Scene scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(scene.name);
    }

    if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextAttackThrowTime && !isAiming)
    {
      nextAttackThrowTime = Time.time + throwRate;
      AttackThrow();
    }

    if (Input.GetKeyDown(KeyCode.F) && canActivateInteractable)
    {
      interactableRequest = true;
    }

    if (Input.GetKeyDown(KeyCode.V) && Time.time >= nextAttackMeleeTime)
    {
      nextAttackMeleeTime = Time.time + meleeRate;
      AttackMelee();
    }

    /// Teleport event activation
    /// 
    if (Input.GetKey(KeyCode.Mouse1) && canTeleport)
    {
      // Show where player will teleport by enabling visuals
      teleportEndpoint.SetActive(true);
      //line.enabled = true;


      isAiming = true;
      // Check raycast
      LayerMask layerMask = LayerMask.GetMask("Default");

      //RaycastHit2D hit2D = Physics2D.Raycast(playerWorldPos, mouseWorldPos, 10, layerMask);
      RaycastHit2D hit2D = Physics2D.Linecast(playerWorldPos, mouseWorldPos, layerMask);
      Collider2D colOverlap = Physics2D.OverlapCircle(mouseWorldPos, 1, layerMask);

      var distance = Vector3.Distance(playerWorldPos, mouseWorldPos);

      if (hit2D.collider != null || colOverlap != null || distance >= teleportMaxDistance) {
        line.material.color = new Color(colorTeleportBlocked.r, colorTeleportBlocked.g, colorTeleportBlocked.b, lineAlpha);
        teleportEndpoint.GetComponent<SpriteRenderer>().material.color = new Color(colorTeleportBlocked.r, colorTeleportBlocked.g, colorTeleportBlocked.b, lineAlpha);
      }
      else {
        line.material.color = new Color(colorTeleportClear.r, colorTeleportClear.g, colorTeleportClear.b, lineAlpha);
        teleportEndpoint.GetComponent<SpriteRenderer>().material.color = new Color(colorTeleportClear.r, colorTeleportClear.g, colorTeleportClear.b, lineAlpha);

        // Teleport on releasing RMB
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
          // Teleport
          teleportTarget = mouseWorldPos;
          StartCoroutine(Teleport());
        }
      }
    } 
    else {
      //var curColor = line.material.color; 
      line.material.color = new Color(0.5f, 0.5f, 0.5f, lineAlpha);
      teleportEndpoint.GetComponent<SpriteRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, lineAlpha);

      isAiming = false;
      teleportEndpoint.SetActive(false);
    }
  }

  void AttackThrow()
  {
    Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
    direction.Normalize();
    GameObject projectile = Instantiate(throwable, player.transform.position, Quaternion.identity);
    projectile.GetComponent<Throwable>().damage = (int)rangeDamage;
    projectile.GetComponent<Rigidbody2D>().velocity = direction * throwForce;

    source.PlayOneShot(sfx_throw);
    mainCamera.GetComponent<StressReceiver>().InduceStress(0.1f);
  }

  void AttackMelee()
  {

    //LayerMask layerMask = LayerMask.NameToLayer("Player");
    bool didHit = false;

    var facingRight = player.GetComponent<PlayerController>().m_FacingRight == true ? 1 : -1;
    var overlapArea = Physics2D.OverlapAreaAll(transform.position, new Vector2(transform.position.x + meleeRange * facingRight, transform.position.y));

    //if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    foreach (Collider2D collider2D in overlapArea)
    {
      if (collider2D.GetComponent<IDamageable>() != null && !collider2D.CompareTag("Player"))
      {
        collider2D.GetComponent<IDamageable>().DealDamage((int)meleeDamage);
        didHit = true;
      }
    }

    mainCamera.GetComponent<StressReceiver>().InduceStress(didHit ? 0.3f : 0.1f);
    source.PlayOneShot(sfx_melee);
  }

  void CheckInteractables()
  {
    Collider2D nearbyInteractable = null;
    var zones = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(3, 2), 0);

    foreach (Collider2D zone in zones) {
      if (zone.CompareTag("Interactable")) {
        nearbyInteractable = zone;
      }
    }

    if (nearbyInteractable != null) {
      var canBeActivated = nearbyInteractable.GetComponent<TriggerArea>().canBeActivated;
      var requiresInput = nearbyInteractable.GetComponent<TriggerArea>().requiresInput;

      if (canBeActivated && requiresInput)
      {
        // show UI hint 
        var values = nearbyInteractable.GetComponent<TriggerArea>();
        managerUI.InteractionPromptSwitchVisibility(true, values.actionDescription);

        canActivateInteractable = true;

        if (interactableRequest) {
          interactableRequest = false;
          canActivateInteractable = false;
          nearbyInteractable.GetComponent<TriggerArea>().Activate();
        }
      }
    }
    else {
      managerUI.InteractionPromptSwitchVisibility(false);
      canActivateInteractable = false;
    }
  }

  IEnumerator Teleport()
  {
    canTeleport = false; teleportTime = 0;
    var _gravityScale = player.GetComponent<Rigidbody2D>().gravityScale;

    player.GetComponent<Rigidbody2D>().gravityScale = 0;
    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    source.PlayOneShot(sfx_teleportIn, 1);

    yield return new WaitForSeconds(teleportDelayTime);

    player.GetComponent<PlayerController>().StopMomentum();
    player.GetComponent<Rigidbody2D>().gravityScale = _gravityScale;
    player.transform.position = teleportTarget;
    
    source.PlayOneShot(sfx_teleportOut);
    mainCamera.GetComponent<StressReceiver>().InduceStress(0.6f);

    yield return null;
  }
}
