import { z } from "zod";
import { Items } from "../items.ts";

export const Attack = z
  .object({
    type: z.literal("attack"),

    itemsUsed: z.enum(Items).array(),

    target: z.string().nullish(),
  })
  .describe(`Player mentions attacking something`);
