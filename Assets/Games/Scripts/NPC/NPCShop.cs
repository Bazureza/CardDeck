using GuraGames.Manager;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.NPC
{
    public class NPCShop : NPCBase
    {
        private ShopManager _shopManager;
        private ShopManager shopManager 
        {
            get
            {
                if (!_shopManager) _shopManager = ServiceLocator.Resolve<ShopManager>();
                return _shopManager;
            }
        }

        protected override bool TryInteract()
        {
            if (isBusy || shopManager.IsBusy()) return false;

            isBusy = true;

            onInteract?.Invoke();
            shopManager.RenderShop();

            isBusy = false;
            return true;
        }
    }
}