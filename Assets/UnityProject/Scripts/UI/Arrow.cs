using UnityEngine;

namespace UnityProject.Scripts.UI
{
    public class Arrow : MonoBehaviour
    {
        private void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}