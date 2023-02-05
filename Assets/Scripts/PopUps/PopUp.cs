using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [Space] 
        [SerializeField] private Text headerText;
    
        public void Show()
        {
            gameObject.SetActive(true);
            UIHelper.showCanvasEvent?.Invoke(canvasGroup, null);
        }

        protected void Close()
        {
            UIHelper.hideCanvasAndDeleteObjEvent(canvasGroup, gameObject);
        }
    
        public void SetHeaderText(string text)
        {
            headerText.text = text;
        }
    }
}
