using DG.Tweening;
using GuraGames.Level;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.GameSystem
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private LevelDataManager _levelDataManager;
        private LevelDataManager levelDataManager
        {
            get
            {
                if (!_levelDataManager) _levelDataManager = ServiceLocator.Resolve<LevelDataManager>();
                return _levelDataManager;
            }
        }

        public Tween MoveCameraToSubLevel(int id)
        {
            GGDebug.Console($"Sub Level ID:{id}");
            var subLevelData = levelDataManager.GetSubLevelData(id);
            return transform.DOMove(subLevelData.GetLevelPosition(), 0.5f).SetEase(Ease.Linear);
        }
    }
}