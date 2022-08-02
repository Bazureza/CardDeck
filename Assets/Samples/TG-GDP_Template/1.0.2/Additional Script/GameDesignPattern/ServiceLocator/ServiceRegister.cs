using UnityEngine;

namespace TomGustin.GameDesignPattern {
    public class ServiceRegister : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour service;

        private void Awake()
        {
            ServiceLocator.Register(service);
        }
    }
}