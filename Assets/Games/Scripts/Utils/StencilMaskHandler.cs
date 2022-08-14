using Sirenix.OdinInspector;
using UnityEngine;

namespace GuraGames.Utility
{
    public class StencilMaskHandler : MonoBehaviour
    {
        [SerializeField] private Material[] materials;

        [Button]
        private void UsingMask()
        {
            foreach (Material material in materials)
            {
                material.SetFloat("_StencilComp", 3);
            }
        }

        [Button]
        private void ReleaseMask()
        {
            foreach (Material material in materials)
            {
                material.SetFloat("_StencilComp", 0);
            }
        }
    }
}