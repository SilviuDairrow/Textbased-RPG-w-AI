import { z } from "zod";
import { Attack } from "./intents/attack.ts";
import { Walk } from "./intents/walk.ts";
import { Sleep } from "./intents/sleep.ts";
import { Spell } from "./intents/spell.ts";

export const Intent = z
  .discriminatedUnion("type", [Attack, Walk, Sleep, Spell])
  .describe(`This schema describes the intent of the user.`);

export const GameSchema = {
  Intent,
  Attack,
  Walk,
  Sleep,
  Spell,
};
