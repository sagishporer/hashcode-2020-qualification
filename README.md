# HashCode 2020 Qualification Solution

Result:
- A: 21
- B: 5822900
- C: 5689354
- D: 5028010
- E: 5061477
- F: 5318970

**Greedy algorithm**
1. Load the dataset.
2. Calculate the score possible for each library (e.g. if sign-in start at current position)
3. Reverse sort by: Score / DaysToSign
4. Select the first library, and remove from libraries list.
5. Remove all the books scanned from all other libraries.
6. Move the current sign-in position start.
7. If any library left, and there are days left - Goto 2.
