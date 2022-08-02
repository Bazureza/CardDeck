using UnityEngine;

namespace TomGustin.GameDesignPattern
{
    public class PointofInterest : Subject
    {
        [SerializeField] private string poiName;

        public override void OnNotify<T>(string sender, T param)
        {
            foreach (IObserver observer in observers) observer.OnNotify(sender, param);
        }
    }
}