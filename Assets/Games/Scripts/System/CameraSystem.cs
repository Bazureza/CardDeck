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
        [SerializeField] private Vector3 offset;
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
            levelDataManager.ChangeActiveSubLevel(id);
            var subLevelData = levelDataManager.GetActiveSubLevelData();
            return transform.DOMove(subLevelData.GetLevelPosition() + offset, 0.5f).SetEase(Ease.Linear);
        }
    }
}