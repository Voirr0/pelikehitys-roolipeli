using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    DoorController activateDoor = null;

    GameObject DoorButtons;
    GameObject DoorPanel;

    //  PANEL ANIMATION
    public RectTransform doorPanel;

    public Vector2 hiddenPos = new Vector2(0, -600);
    public Vector2 visiblePos = new Vector2(0, 0);

    public float slideSpeed = 8f;
    private bool isDoorUIOpen = false;

    public float closeThreshold = 0.1f;

    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        Button openbutton = GameObject.Find("Open").GetComponent<Button>();
        openbutton.onClick.AddListener(OnOpenButton);

        Button Closebutton = GameObject.Find("Close").GetComponent<Button>();
        Closebutton.onClick.AddListener(OnCloseButton);

        Button Lockbutton = GameObject.Find("Lock").GetComponent<Button>();
        Lockbutton.onClick.AddListener(OnLockButton);

        Button OpenLockbutton = GameObject.Find("OpenLock").GetComponent<Button>();
        OpenLockbutton.onClick.AddListener(OnOpenLockButton);

        DoorButtons = GameObject.Find("DoorButtons");
        DoorButtons.SetActive(false);

        DoorPanel = GameObject.Find("DoorPanel");
        DoorPanel.SetActive(false);

        //  initialize animation
        doorPanel.anchoredPosition = hiddenPos;
        isDoorUIOpen = false;
    }

    void OnOpenButton()
    {
        activateDoor.ReceiveAction(DoorController.Toiminnot.avaa);

        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.OpenDoor);
    }

    void OnCloseButton()
    {
        activateDoor.ReceiveAction(DoorController.Toiminnot.sulje);
    }

    void OnLockButton()
    {
        activateDoor.ReceiveAction(DoorController.Toiminnot.lukitse);
    }

    void OnOpenLockButton()
    {
        activateDoor.ReceiveAction(DoorController.Toiminnot.poistalukitus);
    }

    void Update()
    {
        //  PANEL SLIDE ANIMATION
        Vector2 target = isDoorUIOpen ? visiblePos : hiddenPos;

        doorPanel.anchoredPosition = Vector2.Lerp(
            doorPanel.anchoredPosition,
            target,
            Time.deltaTime * slideSpeed
        );

        // auto disable after sliding out
        if (!isDoorUIOpen && DoorPanel.activeSelf)
        {
            float distance = Vector2.Distance(doorPanel.anchoredPosition, hiddenPos);

            if (distance < closeThreshold)
            {
                DoorPanel.SetActive(false);
                DoorButtons.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Door"))
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundEffect.HitWall);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            activateDoor = collision.GetComponent<DoorController>();

            DoorPanel.SetActive(true);
            DoorButtons.SetActive(true);

            //  trigger slide in
            isDoorUIOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            //  trigger slide out
            isDoorUIOpen = false;
        }
    }

    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }
}