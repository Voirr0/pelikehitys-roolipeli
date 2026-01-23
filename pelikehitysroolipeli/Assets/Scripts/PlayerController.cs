using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;
    DoorController activateDoor = null;
    GameObject DoorButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
    }

    void OnOpenButton()
    {
        Debug.Log("Open button was pressed");
        activateDoor.ReceiveAction(DoorController.Toiminnot.avaa);
    }

    void OnCloseButton()
    {
        Debug.Log("Close button was pressed");
        activateDoor.ReceiveAction(DoorController.Toiminnot.sulje);
    }

    void OnLockButton()
    {
        Debug.Log("Lock button was pressed");
        activateDoor.ReceiveAction(DoorController.Toiminnot.lukitse);
    }

    void OnOpenLockButton()
    {
        Debug.Log("OpenLock button was pressed");
        activateDoor.ReceiveAction(DoorController.Toiminnot.poistalukitus);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Huomaa mitä pelaaja löytää
        if (collision.CompareTag("Door"))
        {
            Debug.Log("Found Door");
            activateDoor = collision.GetComponent<DoorController>();


            //napit näkyviin
            DoorButtons.SetActive(true);    

        }
        else if (collision.CompareTag("Merchant"))
        {
            Debug.Log("Found Merchant");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            DoorButtons.SetActive(false);
        }
            
    }



    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }    
}
