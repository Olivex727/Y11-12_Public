## C# TradeGame Algorithm (NPCs):

- Test each available option 3 turns ahead
- Return a number value of the net money made
- Put all values on a list
- Use intelligence and RNG to determine option
- Perform option

### OPTIONS:
- **foreach available Material (includes Facilities):**
  - Buy Material (x amount)
  - Sell Material (x amount)

[Use inflation and amount, quadratic rate of change to calculate the max amount of material needed and money from that]

- **foreach accessible Market:**
  - Go to Market

[Use netWorth and markup to calculate the max amount of money earned]

- **foreach unowned Facility:**
  - Buy/Develop Facility

[Use the prod, cost and mcost to find the most money made]

- **foreach owned Facility:**
  - Sell Facility
  - Store Material (x amount)
  - Withdraw Material (x amount)

[Use inflation and amount, quadratic rate of change to calculate the max amount of material needed and money from that],
[Use the prod, cost and mcost to find the most money made]

- **foreach NPC:**
 - Sponsor
 - Merger (of same type)

[use ∆netWorth to find out the best choice, Merger if it's really good]

- Go Public (x amount shares)

[Use potential cost of shares to figure out max value]

### INTELLIGENCE RNG:

- Put all options on scale from 1 - x
- Use rnd.Next(x * (intelligence /100), x)

[higher intelligence means higher values]

- Make sure that if the inventory value of the NPC is <= 0, do a NPC.liquidate() function

- The choice is the corresponding rnd value

### FORMULAE:

A = Amount available (Upper Bound)
A0 = Amount Owned (Negative Lower Bound)
c = Cost (Lower cost, more to buy)
i = Inflation (Higher inflation, more to buy)
id = Change in inflation (Higher id, more to buy)

int x = -A + (A * (i+id) / Math.Pow(c, 1/2));

x(c/C) > a/2

make xc = a/2

x = aC/2c

x(c/C) > m
x = m(C/c)
