import { z } from "zod";

export const Spell = z.object({
  type: z.literal("spell"),

  target: z.string().nullish(),
}).describe(`
    Player mentions using a spell on something.
    Player attacks with magic a target.
    Player uses magic on something.
  `);
