import { createJsonTranslator, createOpenAILanguageModel } from "typechat";
import { createZodJsonValidator } from "typechat/zod";
import { GameSchema } from "./interfaces/game.ts";
import "@std/dotenv/load";
import OpenAI from "openai";
import { decodeBase64 } from "@std/encoding/base64";
import { Hono } from "hono";

const OPENAI_API_KEY = Deno.env.get("OPENAI_API_KEY")!;
const OPENAI_LLM = Deno.env.get("OPENAI_LLM")!;
const OPENAI_IMAGE = Deno.env.get("OPENAI_IMAGE")!;

const openapiClient = new OpenAI({
  apiKey: OPENAI_API_KEY,
});

const model = createOpenAILanguageModel(OPENAI_API_KEY, OPENAI_LLM);

const validator = createZodJsonValidator(GameSchema, "Intent");
const translator = createJsonTranslator(model, validator);

const app = new Hono();

app.post("/prompt", async (c) => {
  const { query, context } = await c.req.json();

  const response = await translator.translate(query);

  if (
    response.success &&
    (response.data.type === "attack" || response.data.type === "spell")
  ) {
    const actionDescription = `
    Original prompt: ${query}
    Parsed prompt: ${response} 
    `;

    const {
      choices: [
        {
          message: { content: positiveContent },
        },
      ],
    } = await openapiClient.chat.completions.create({
      model: OPENAI_LLM,
      messages: [
        {
          role: "system",
          content: `
            Describe the following information VERY detailed for DALLE-3 IMAGE GENERATION.
            Don't forget any details when describing. Pay more attention to 'Parsed prompt' than to 'Original prompt'.
            Respond ONLY with the description. Don't add any unnecessary text.
            Describe the position of every element in the image and every important detail.
             
  
            <Details>
            The scene happens in a dark forest at night.
  
            Michael The Wizard is the player. The person doing the actions.
  
            Michael The Wizard has blue robes and is a tall skinny white old man.
  
            Arthur The Goblin is a red goblin with spikes on his back and a very creepy, demonic smile.
            </Details>
  
            Only use information from <Details /> IF the <AdditionalContext /> asks for it.
  
            <AdditionalContext>
            ${context}
            </AdditionalContext>
            `,
        },
        {
          role: "user",
          content: `
            <Information>
            ${actionDescription}
            </Information>
          `,
        },
      ],
    });

    const {
      choices: [
        {
          message: { content: negativeContent },
        },
      ],
    } = await openapiClient.chat.completions.create({
      model: OPENAI_LLM,
      messages: [
        {
          role: "system",
          content: `
            Starting from the following description list what SHOULDN'T/COULDN'T BE in the following description.
            List/create any elements that won't fit the description. List negative prompts for dalle-3 image generation.
            Include a lot of details. Be very artistic. Style everything that comes to mind. Mention every possible details.
            Respond ONLY with the elements separated by commas. Don't say anything unnecessary.
          `,
        },
        {
          role: "user",
          content: `
            <Description>
            ${positiveContent}
            </Description>
          `,
        },
      ],
    });

    const img = await imgGen(positiveContent!, negativeContent!);

    return c.json({
      response,
      image: img,
    });
  }

  return c.json({
    response,
  });
});

Deno.serve(app.fetch);

async function imgGen(
  positiveContent: string,
  negativeContent: string
): Promise<string> {
  const response = await fetch("http://192.168.100.82:7860/sdapi/v1/txt2img", {
    method: "POST",
    headers: {
      accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      prompt: positiveContent,
      negative_prompt: negativeContent,
      seed: 1,
      send_images: true,
      save_images: true,
    }),
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  const { images } = await response.json();

  return images[0];
}
