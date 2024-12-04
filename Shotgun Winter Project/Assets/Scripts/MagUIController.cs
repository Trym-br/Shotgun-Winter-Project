using UnityEngine;
using UnityEngine.UI;

public class MagUIController : MonoBehaviour
{
        [SerializeField] private Sprite _emptyShell;
        [SerializeField] private Sprite _liveShell;
    
        private int index = 0;
        public void UpdateMagUI(int ammo)
        {
            foreach (Transform child in transform)
            {
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
            index = 0;
        }

}
