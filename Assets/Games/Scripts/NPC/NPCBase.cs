using GuraGames.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace GuraGames.NPC
{
    public class NPCBase : MonoBehaviour, IInteract
    {
        [SerializeField] protected UnityEvent onInteract;

        protected bool isBusy;

        protected virtual bool TryInteract()
        {
            if (isBusy) return false;

            isBusy = true;
            onInteract?.Invoke();
            isBusy = false;
            return true;
        }

        bool IInteract.TryToInteract()
        {
            return TryInteract();
        }
    }
}