Player State
============

### Equipment

- Head
- Outfit
- Feet
- Accessory
- Hands?

Head, outfit, and feet will have stats. Since they're bigger/smaller those
states will be affected by some multiplier.
e.g. a heavy outfit is heavier than a heavy helmet.

### Warmth

- freezing: -3
- cold: -2
- chilly: -1
- neutral: 0
- warm: 1
- hot: 2
- boiling: 3
  
These states have different effects:

- gradual damage: cold, hot
- fast damage: freezing, boiling

Possible thirst penalty for heat.
Possible movement penalty for cold.
  
### Protection

- naked: 0
- clothed: 1
- protected: 2

Gives a defense bonus when attacked.
Being naked might cause certain bad cutscenes.
(e.g. Can't be naked around city guards.)

### Weight

- None: 0
- Light: 1
- Moderate: 2
- Heavy: 3

Weight affects how much energy is expended by moving.
Heavy weight negatively affects movement range.

### Energy

- passed out: -3
- exhausted: -2
- tired: -1
- rested: 0

Being exhausted pushes down your health.
Passing out negatively impacts your health.

### Hunger

- Starving: -2
- Hungry: -1
- Satisfied: 0

Being starving pushes down your health.

### Thirst

- Parched: -2
- Thirsty: -1
- Satisfied: 0

Begin parched pushes down your health.

### Health

Two separate trees: physical injury vs. sickness.
Physical injury takes precedence over sickness:
if you're sick and get hurt, well now you're just hurt.

- Healthy
- Unwell -> sick -> very sick
- Injured -> gravely injured
