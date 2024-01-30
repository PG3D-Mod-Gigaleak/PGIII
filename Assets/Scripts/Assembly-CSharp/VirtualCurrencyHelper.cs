using System.Collections.Generic;

public class VirtualCurrencyHelper
{
	public static int[] coinInappsQuantity;

	public static Dictionary<string, int> prices;

	static VirtualCurrencyHelper()
	{
		coinInappsQuantity = new int[7] { 15, 30, 45, 80, 165, 330, 800 };
		prices = new Dictionary<string, int>
        {
            { StoreKitEventListener.crystalswordID, 75 },
            { StoreKitEventListener.fullHealthID, 15 },
            { StoreKitEventListener.bigAmmoPackID, 15 },
            { StoreKitEventListener.minerWeaponID, 30 },
            { StoreKitEventListener.elixirID, 30 },
            { StoreKitEventListener.combatrifle, 75 },
            { StoreKitEventListener.magicbow, 150 },
            { StoreKitEventListener.goldeneagle, 30 },
            { StoreKitEventListener.chief, 45 },
            { StoreKitEventListener.nanosoldier, 45 },
            { StoreKitEventListener.endmanskin, 30 },
            { StoreKitEventListener.spaceengineer, 45 },
            { StoreKitEventListener.steelman, 45 },
            { StoreKitEventListener.CaptainSkin, 45 },
            { StoreKitEventListener.HawkSkin, 45 },
            { StoreKitEventListener.TunderGodSkin, 45 },
            { StoreKitEventListener.GreenGuySkin, 45 },
            { StoreKitEventListener.GordonSkin, 45 },
            { StoreKitEventListener.axe, 45 },
            { StoreKitEventListener.spas, 60 },
            { StoreKitEventListener.armor, 30 },
            { StoreKitEventListener.chainsaw, 75 },
            { StoreKitEventListener.famas, 75 },
            { StoreKitEventListener.glock, 45 },
            { StoreKitEventListener.scythe, 60 },
            { StoreKitEventListener.shovel, 30 },
            { StoreKitEventListener.hammer, 100 },
            { StoreKitEventListener.sword_2, 150 },
            { StoreKitEventListener.staff, 180 },
            { StoreKitEventListener.magicGirl, 45 },
            { StoreKitEventListener.braveGirl, 45 },
            { StoreKitEventListener.glamDoll, 45 },
            { StoreKitEventListener.kittyGirl, 45 },
            { StoreKitEventListener.famosBoy, 75 }
        };
	}
}
