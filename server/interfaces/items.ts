import { ConsumableItems } from "./items/consumables.ts";
import { WeaponItems } from "./items/weapons.ts";

export const Items = [...ConsumableItems, ...WeaponItems] as const;
