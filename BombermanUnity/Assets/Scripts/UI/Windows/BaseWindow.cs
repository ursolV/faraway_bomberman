using UnityEngine;

namespace UI
{
    public class BaseWindow : MonoBehaviour
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void SetParam(object param)
        {
            
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
