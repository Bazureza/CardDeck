using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GuraGames.Utility
{
    public class StencilMaskHandler : MonoBehaviour
    {
        [SerializeField] private Material[] materials;

        [SerializeField] private Transform[] rendererSpriteParent;

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

        [Button]
        private void UsingMaskRenderer()
        {
            foreach (Transform transform in rendererSpriteParent)
            {
                foreach (SpriteRenderer render in transform.GetComponentsInChildren<SpriteRenderer>())
                {
                    render.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                }

                foreach (TilemapRenderer render in transform.GetComponentsInChildren<TilemapRenderer>())
                {
                    render.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                }
            }
        }

        [Button]
        private void ReleaseMaskRenderer()
        {
            foreach (Transform transform in rendererSpriteParent)
            {
                foreach (SpriteRenderer render in transform.GetComponentsInChildren<SpriteRenderer>())
                {
                    render.maskInteraction = SpriteMaskInteraction.None;
                }

                foreach (TilemapRenderer render in transform.GetComponentsInChildren<TilemapRenderer>())
                {
                    render.maskInteraction = SpriteMaskInteraction.None;
                }
            }
        }
    }
}