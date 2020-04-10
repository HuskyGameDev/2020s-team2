using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvasObject;
    [SerializeField] private RectTransform popupObject;
    [SerializeField] private Text textString;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float padding;

    private Canvas popupCanvas;

    private void Awake()
    {
        popupCanvas = popupCanvasObject.GetComponent<Canvas>();
        popupCanvasObject.SetActive(false);

        //Stops warnings
        if (popupCanvasObject == null)
        {
            popupCanvasObject = null;
        }
        if (popupCanvas == null)
        {
            popupCanvas = null;
        }
        if (popupObject == null)
        {
            popupObject = null;
        }
       if (textString == null)
        {
            textString = null;
        }
       if (offset == null)
        {
            offset = new Vector3();
        }
        if (padding == 0)
        {
            padding = 0;
        }
    }

    private void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        if (!popupCanvasObject.activeSelf) { return; }

        Vector3 newPos = Input.mousePosition + offset;
        newPos.z = 0f;
        float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
        if (rightEdgeToScreenEdgeDistance < 0)
        {
            newPos.x += rightEdgeToScreenEdgeDistance;
        }
        float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
        if (leftEdgeToScreenEdgeDistance > 0)
        {
            newPos.x += leftEdgeToScreenEdgeDistance;
        }
        float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
        if (topEdgeToScreenEdgeDistance < 0)
        {
            newPos.y += topEdgeToScreenEdgeDistance;
        }
        popupObject.transform.position = newPos;
    }

    public void DisplayText(string name)
    {
        if (name.Equals("StaffWhack"))
        {
            textString.text = "A standard melee attack against an adjacent foe that deals two damage";
        }
        else if (name.Equals("FiendishWisp"))
        {
            textString.text = "A projectile attack with a range of three. It deals one damage to the\nfirst enemy it hits, and it can only travel in a straight line";
        }
        else if (name.Equals("ViciousSlap"))
        {
            textString.text = ("A melee attack that deals one damage and knocks the enemy back one space");
        }
        else if (name.Equals("ForgottenCurse"))
        {
            textString.text = "A melee attack against an adjacent enemy prevents them from moving on\ntheir next turn";
        }
        else if (name.Equals("InfernalBubble"))
        {
            textString.text = "Creates a shield around either you or your sacrifice that lasts for one\nturn. It nullifies the next attack that your unit would take";
        }
        else if (name.Equals("ShadySwitcheroo"))
        {
            textString.text = "Makes the sacrifice and necromancer to switch places on the map";
        }
        else if (name.Equals("ShadowStep"))
        {
            textString.text = "Increases the movement range of both the necromancer and the sacrifice by\none space for the next turn";
        }
        else
        {
            textString.text = "No desctiption available";
        }
        popupCanvasObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void HideText()
    {
        popupCanvasObject.SetActive(false);
    }
}
