﻿# Spec for Monogame.Extended.Spritesheet

## TextureAtlas
> Example
  "textureAtlas": {
    "texture": "pig-sprite-sheet.png",
    "regionWidth": 32,
    "regionHeight": 32
  },

## Cycles
> Example
"cycles": {
    "oink": {
      "frames": [0,1,2,3],
      "isLooping": false
    },
    "bounce": {
      "frames": [8,9,10,11,12,13,14,15],
      "isLooping": false
    },
    "jump": {
      "frames": [16,17,18,19,20],
      "isLooping": false
    },
    "sleep": {
      "frames": [24,25,26,27,28,29,30],
      "isLooping": false
    }
  }

### Fields for Animation cycle
 bool IsLooping
 bool IsReversed
 bool IsPingPong
 float FrameDuration = 0.2f
 > List Frames
     int Index
     float Duration = 0.2f

