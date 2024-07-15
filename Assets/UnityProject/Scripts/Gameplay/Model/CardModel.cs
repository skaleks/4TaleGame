namespace UnityProject.Scripts.Gameplay.Model
{
    public class CardModel
    {
        public CardData CardData { get; }
        public CardModel(CardData cardData)
        {
            CardData = cardData;
        }
    }
}