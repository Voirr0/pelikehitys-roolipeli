using UnityEngine;
using UnityEngine.Rendering;
using static DoorController;

public class DoorController : MonoBehaviour
{

   public enum Oventila
    {
        auki,
        kiinni,
        lukossa
    }

   public enum Toiminnot
    {
        avaa,
        sulje,
        lukitse,
        poistalukitus
    }


    // Kuvat oven eri tiloille
    [SerializeField]
    Sprite ClosedDoorSprite;
    [SerializeField]
    Sprite OpenDoorSprite;
    [SerializeField]
    Sprite LockedSprite;
    [SerializeField]
    Sprite UnlockedSprite;

    BoxCollider2D colliderComp;

    // Näitä värejä käytetään lukkosymbolin piirtämiseen.
    public static Color lockedColor;
    public static Color openColor;

    SpriteRenderer doorSprite; // Oven kuva
    SpriteRenderer lockSprite; // Lapsi gameobjectissa oleva lukon kuva

    // Debug ui
    [SerializeField]
    bool ShowDebugUI;
    [SerializeField]
    int DebugFontSize = 32;

    Oventila oven_tila;

    void Start()
    {
        doorSprite = GetComponent<SpriteRenderer>();
        colliderComp = GetComponent<BoxCollider2D>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 2 && sprites[0] == doorSprite)
        {
            lockSprite = sprites[1];
        }

        
        lockedColor = new Color(1.0f, 0.63f, 0.23f);
        openColor = new Color(0.5f, 0.8f, 1.0f);


        
        oven_tila = Oventila.lukossa;
        
    }

   
    public void ReceiveAction(Toiminnot oven_toiminnot)
    {
        if (oven_toiminnot == Toiminnot.sulje && oven_tila == Oventila.auki)
        {
            oven_tila = Oventila.kiinni;
            CloseDoor();
        }

        else if (oven_toiminnot == Toiminnot.avaa && oven_tila == Oventila.kiinni)
        {
            oven_tila = Oventila.auki;
            OpenDoor();
        }

        else if (oven_toiminnot == Toiminnot.lukitse && oven_tila == Oventila.kiinni)
        {
            oven_tila = Oventila.lukossa;
            LockDoor();
        }

        else if (oven_toiminnot == Toiminnot.poistalukitus && oven_tila == Oventila.lukossa)
        {
            oven_tila = Oventila.kiinni;
            UnlockDoor();
        }
    }

    
    private void OpenDoor()
    {
        doorSprite.sprite = OpenDoorSprite;
        colliderComp.isTrigger = true;
    }

    
    private void CloseDoor()
    {
        doorSprite.sprite = ClosedDoorSprite;
        colliderComp.isTrigger = false;
    }

    
    private void LockDoor()
    {
        lockSprite.sprite = LockedSprite;
        lockSprite.color = lockedColor;
    }

    
    private void UnlockDoor()
    {
        lockSprite.sprite = UnlockedSprite;
        lockSprite.color = openColor;
    }

    
    private void OnGUI()
    {
        if (ShowDebugUI == false)
        {
            return;
        }
        GUIStyle buttonStyle = GUI.skin.GetStyle("button");
        GUIStyle labelStyle = GUI.skin.GetStyle("label");
        buttonStyle.fontSize = DebugFontSize;
        labelStyle.fontSize = DebugFontSize;
        Rect guiRect = GetGuiRect();
        GUILayout.BeginArea(guiRect);
        
        GUILayout.Label("Door");
        if (GUILayout.Button("Open"))
        {
            OpenDoor();
        }
        if (GUILayout.Button("Close"))
        {
            CloseDoor();
        }
        if (GUILayout.Button("Lock"))
        {
            LockDoor();
        }
        if (GUILayout.Button( "Unlock"))
        {
            UnlockDoor();
        }
        
        GUILayout.EndArea();
    }

   
   

    private Rect GetGuiRect()
    {
        Vector3 buttonPos = transform.position;
        buttonPos.x += 1;
        buttonPos.y -= 0.25f;
        
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(buttonPos);
        float screenHeight = Screen.height;
        return new Rect(screenPoint.x, screenHeight - screenPoint.y, 
            DebugFontSize * 8,  
            DebugFontSize * 100);
    }    
}
