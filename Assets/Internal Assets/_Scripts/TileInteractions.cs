using UnityEngine;

public class TileInteractions : MonoBehaviour, IInteractable
{
    private TileScript _script;

    private void Start()
    {
        _script = gameObject.GetComponent<TileScript>();
    }

    public void Interact()
    {
        // UnitControllerBase.Instance.MoveToTile(_script);
    }
}
