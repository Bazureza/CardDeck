using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private bool usingCanvas = true;
        [SerializeField, ShowIf("usingCanvas")] private Canvas healthCanvas;
        [SerializeField] private Image health;
        [SerializeField] private TextMeshProUGUI health_value;
        [SerializeField] private Image shieldIcon;

        public void UpdateHealth(int base_health, int current_health)
        {
            var proportion = current_health / (float)base_health;

            if (health_value) health_value.text = $"{current_health}/{base_health}";
            if (usingCanvas && healthCanvas) healthCanvas.enabled = proportion != 1f;
            if (!usingCanvas || healthCanvas.enabled) health.fillAmount = proportion;
        }

        public void SetShieldIcon(bool active)
        {
            shieldIcon.enabled = active;
        }
    }
}