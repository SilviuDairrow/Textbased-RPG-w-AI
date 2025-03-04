import { z } from "zod";

const Direction = z.enum(["left", "right", "up", "down"]);

export const Walk = z
  .object({
    type: z.literal("walk"),

    direction: Direction,
  })
  .describe(`Player mentions walking somewhere`);
