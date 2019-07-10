## Pacific Rose Lanes.

Hello, Pacific Rose Lanes is classic bowling alley offering family entertainment in the heart of an
historic Pacific Northwest town. We still have wooden lanes and bowling shoes (and milk shakes!) 
reminiscent of the 1950's. We also do NOT have automatic scoring machines. This hasn't been a problem
since many folks who bowl here are used to keeping score the old-fashioned way.

One of our younger bowlers, who happens to program computers, thought it would be fun to score games
with a computer program. So, they wrote this program and since have gone off to college. So we are
left to kep it up-to-date ourselves. Any help is appreciated!

The program takes a list of strings representing a 10-frame game that look like this:
- "16|5/|54|3/|5/|X|X|X|6/|X||X5"

The optional "bonus throws" in the 10th frame follow the '||' delimiter above. It's kind of funky,
but we love this old syntax :heart:, so we just keep it as-is when changing the program :smile:.

## Scoring Rules

Frames consist of 2 'balls', or throws, and can be filled as 'open' (no spares or strikes), in which case,
the frame's score can be immediately calculated by adding the total pins knocked down to 
the previous score total.

If a frame has a 'mark' (a spare or a strike).  The spare '/' counts as 10 plus the pins knocked down on 
the next ball thrown in the next frame. 

The strike 'X' counts as 10 plus the next 2 balls thrown in the next frame(s). Yes, if you get 3 strikes
in a row, that is three frames in three throws - so the first strike scores 30. This makes it a little tricky. 

The tenth frame is special as you get a 'bonus' throw if you happen to get a spare, and two 'bonus' throws
if you happen to get a strike - maximum of three throws in the 10th frame. Spares and strike award
bonus pins in the usual way, but if there are no more throws to count, they just count as 10 each. So,
a "three bagger" (or 'XXX') is worth 30 in the 10th frame.

Using these rules on the example game above, you should arrive at a score of 187 - not bad!

## Getting Started
Clone the repository and open the *BowlingScore.sln* file in Visual Studio, or your editor/IDE of choice.
This is a dotnet core project, so you can also use the command line 'dotnet build' and 'dotnet run' commands 
in the console project and test project directories respectively.

## Re-Factoring
The project comes with tests written, so you can start re-factoring right away, and re-runs tests
to insure things are on-track.

The old "game syntax" shown above is a little confusing the way the bonus throws are expressed, but
the program parses the old syntax so games scored on the original program still score correctly. So, you
can't change the syntax as part of re-factoring.

### Re-Factor Palooza
You have choices!!!
- You can start with the "Old Program" tests and start re-factoring from ground-zero. The original 
*OldProgram.cs* and *OldProgramTests.cs* are provided. 
- You can start with the "New Program" tests (*BowlingTests.cs*) and start re-factoring from there.

Either or both choices of starting point will allow you to have some good practice and fun. 

The example *Program.cs* and tests *BowlingTests.cs* have been re-factored from the orginal program.
Still, there is more to be done and we'd love to see and hear about improvements.

Some example refinements:
- Right now, the program just provides a final score of a full 10-frame game. It might be nice to get
a running total.
- The scoring rules are a bunch of if-statements. It might be possible to clean that up a little. Frames know
how to score themselves, but the semantics of scoring a 10-frame game requires knowing how future frames are
going to be played. 

## License

MIT

## Suggested Attribution
This work is by Mike O'Shea [@oshea00](https://twitter.com/oshea00)


## Acknowledgements
This work was inspired by the [Gilded Rose Project](https://github/NotMyself/GildedRose)


