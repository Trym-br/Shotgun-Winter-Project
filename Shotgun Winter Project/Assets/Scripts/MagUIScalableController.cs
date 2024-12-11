using System;
using UnityEngine;
using UnityEngine.UI;

public class MagUIScalableController : MonoBehaviour
{
    [SerializeField] private Sprite _beltStart;
    [SerializeField] private Sprite _beltEnd;
    [SerializeField] private Sprite _liveShell;
    [SerializeField] private Sprite _emptyShell;
        
    [SerializeField] private float _sizemodifier;
    [Header("Only for Debug")]
    [SerializeField] private float _spacing;
    [SerializeField] private float _offset;
    private RectTransform _rectTransform;
    private GameObject _parentRef;
    // [SerializeField] private int _magSizeLocal;
    public void InitUI(int _magSize)
    {
        // _magSizeLocal = _magSize;
        _spacing = _liveShell.rect.width;
        // print("spacing: " + _spacing + " | rect size: " + _liveShell.rect.width);
        Sprite sprite = _liveShell;
        GameObject ParentRef = GameObject.Find("Mag UI Scaleable");
        _parentRef = ParentRef;
        _rectTransform = ParentRef.GetComponent<RectTransform>();
        
        // Belt start
        GameObject BeltStartRef = new GameObject("BeltStart");
        BeltStartRef.transform.SetParent(ParentRef.transform);
        Image BeltStartImage = BeltStartRef.AddComponent<Image>();
        RectTransform BeltStartRect = BeltStartRef.GetComponent<RectTransform>();
        BeltStartImage.sprite = _beltStart;
        BeltStartRect.anchorMin = new Vector2(0, 0.5f);
        BeltStartRect.anchorMax = new Vector2(0, 0.5f);
        BeltStartRect.anchoredPosition = new Vector2(0, 0);
        BeltStartRect.sizeDelta = new Vector2(_sizemodifier * _beltStart.rect.width, _sizemodifier * _beltStart.rect.height);
        BeltStartRect.localScale = new Vector3(1, 1, 1);
        BeltStartRect.pivot = new Vector2(0, 0.5f);
        _offset += _beltStart.rect.width;
        
        // Bullets
        for (int i = 0; i < _magSize; i++)
        {
            GameObject ChildRef = new GameObject("Shell " + i);
            ChildRef.transform.SetParent(ParentRef.transform);
            Image ChildImage = ChildRef.AddComponent<Image>();
            RectTransform ChildRect = ChildRef.GetComponent<RectTransform>();
            ChildImage.sprite = sprite;
            ChildRect.anchorMin = new Vector2(0, 0.5f);
            ChildRect.anchorMax = new Vector2(0, 0.5f);
            // ChildRect.anchoredPosition = new Vector2(_offset + _spacing * i, 0);
            // print(i + ": spacing = " + _spacing + " / _offset = " + _offset + " / size modifier = " + _sizemodifier);
            ChildRect.anchoredPosition = new Vector2(_offset * _sizemodifier + _spacing * _sizemodifier * i, 0);
            ChildRect.sizeDelta = new Vector2(_sizemodifier * sprite.rect.width, _sizemodifier * sprite.rect.height);
            ChildRect.localScale = new Vector3(1, 1, 1);
            ChildRect.pivot = new Vector2(0, 0.5f);
            // print("Bullet " + i + ": " + new Vector2(_sizemodifier * sprite.rect.width, _sizemodifier * sprite.rect.height));
        }
        
        // Belt end
        GameObject BeltEndRef = new GameObject("BeltEnd");
        BeltEndRef.transform.SetParent(ParentRef.transform);
        Image BeltEndImage = BeltEndRef.AddComponent<Image>();
        RectTransform BeltEndRect = BeltEndRef.GetComponent<RectTransform>();
        BeltEndImage.sprite = _beltEnd;
        BeltEndRect.anchorMin = new Vector2(0, 0.5f);
        BeltEndRect.anchorMax = new Vector2(0, 0.5f);
        BeltEndRect.anchoredPosition = new Vector2(_offset * _sizemodifier + _spacing * _sizemodifier * _magSize, 0);
        BeltEndRect.sizeDelta = new Vector2(_sizemodifier * _beltEnd.rect.width, _sizemodifier * _beltEnd.rect.height);
        BeltEndRect.localScale = new Vector3(1, 1, 1);
        BeltEndRect.pivot = new Vector2(0, 0.5f);
    }
        
    public void UpdateMagUI(int ammo)
    { 
        if (_parentRef == null) { return; }
        int index = 0;
        foreach (Transform child in _parentRef.transform)
        {
            if (child.name == "BeltStart" || child.name == "BeltEnd") { continue; }
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

    private PlayerController _playerRef;
    public void OnDrawGizmos()
    {
        if (_playerRef == null) { _playerRef = GameObject.Find("Player").GetComponent<PlayerController>(); }
        Gizmos.color = Color.yellow;
        Vector3 totalSize = new Vector3(1,1,1);
        totalSize.x =
            (_beltStart.rect.width * _sizemodifier) +
            (_liveShell.rect.width * _sizemodifier * _playerRef.MagSize) +
            (_beltEnd.rect.width * _sizemodifier)
            ;
        totalSize.y = _beltStart.rect.height * _sizemodifier;
        // ??? divided by pixel to unit amount and by _sizemodifier, effectively becoming raw asset pixels divided by pixels pr unit?
        // but it lines up pixel perfect??
        totalSize.x /= 32;
        totalSize.y /= 32;
        totalSize.x /= 3;
        totalSize.y /= 3;
        // print("totalsize: " + totalSize);
        Gizmos.DrawWireCube(transform.position + new Vector3(totalSize.x/2, 0, 0), totalSize);
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.DrawCube(transform.position + new Vector3(totalSize.x/2, 0, 20), totalSize);
        }
    }
}