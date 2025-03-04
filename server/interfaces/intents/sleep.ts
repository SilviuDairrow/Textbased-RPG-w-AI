import { z } from "zod";

export const Sleep = z
  .object({
    type: z.literal("sleep"),
  })
  .describe(`Player mentions sleeping or resting or regenerating stamina`);
