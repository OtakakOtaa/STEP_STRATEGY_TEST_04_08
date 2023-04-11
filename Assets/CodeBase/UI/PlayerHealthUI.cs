using UnityEngine;

namespace CodeBase.UI
{
    public sealed class PlayerHealthUI : MonoBehaviour
    {
        public string Health = "100";

        public void ChangeHeath(int amount)
            => Health = amount.ToString();
    }
}