using UnityEngine;
using UnityEngine.EventSystems;

public class StealItemUIHandler : MonoBehaviour, IPointerClickHandler
{
    public StealItem _item;
    public bool _isActivated = false;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(_isActivated)
        {
            Debug.Log("Clicked!" + _item.name);
            FindObjectOfType<StealManager>().RemoveItem(_item);
        }
    }
}
