using UnityEngine;
using UnityEngine.UI;

public class MagUIController : MonoBehaviour
{
    [SerializeField] private Sprite _emptyShell;
    [SerializeField] private Sprite _liveShell;
        
    [SerializeField] private float _spacing;
    [SerializeField] private float _offset;
    [SerializeField] private int _magSize;
    [SerializeField] PlayerController _player;
    [SerializeField] private float _sizemodifier;
    private RectTransform _rectTransform;
    private GameObject _parentRef;

    void Start()
    {
        InitUI();
    }
        
    public void InitUI()
    {
        if (_player != null) { _magSize = _player.MagSize; }
        Sprite sprite = _liveShell;
        GameObject ParentRef = GameObject.Find("Mag UI");
        _parentRef = ParentRef;
        _rectTransform = ParentRef.GetComponent<RectTransform>();
        print(ParentRef);
        for (int i = 0; i < _magSize; i++)
        {
            GameObject ChildRef = new GameObject("Shell " + i);
            ChildRef.transform.SetParent(ParentRef.transform);
            Image ChildImage = ChildRef.AddComponent<Image>();
            RectTransform ChildRect = ChildRef.GetComponent<RectTransform>();
            ChildImage.sprite = sprite;
            ChildRect.anchorMin = new Vector2(0, 0.5f);
            ChildRect.anchorMax = new Vector2(0, 0.5f);
            ChildRect.anchoredPosition = new Vector2(_offset + _spacing * i, 0);
            ChildRect.sizeDelta = new Vector2(_sizemodifier * sprite.rect.width, _sizemodifier * sprite.rect.height);
            ChildRect.localScale = new Vector3(1, 1, 1);
            // print("Bullet " + i + ": " + new Vector2(_sizemodifier * sprite.rect.width, _sizemodifier * sprite.rect.height));
        }
    }
        
    public void UpdateMagUI(int ammo)
    { 
        if (_parentRef == null) { return; }
        int index = 0;
        foreach (Transform child in _parentRef.transform)
        {
            if (child.name == "Pouch") {continue;}
            if (index < ammo)
            {
                child.gameObject.GetComponent<Image>().sprite = _liveShell;
            }
            else
            {
                child.gameObject.GetComponent<Image>().sprite = _emptyShell;
            }
            index += 1;
        }
    }
}