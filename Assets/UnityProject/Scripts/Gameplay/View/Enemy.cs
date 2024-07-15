using UnityEngine.EventSystems;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.UI;

namespace UnityProject.Scripts.Gameplay.View
{
    public class Enemy : Character, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public EnemyData EnemyData { get; private set; }

        public void Initialize(EnemyData enemyData)
        {
            EnemyData = enemyData;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale *= 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale /= 1.1f;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.lastPress.GetComponent<Card>() != null)
            {
                eventData.lastPress.GetComponent<Card>().Target = this;
            }
        }
    }
}