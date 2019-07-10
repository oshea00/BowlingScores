using BowlingScore;
using NUnit.Framework;
using System;
using System.IO;

namespace OldProgramTests
{
    class OldProgramTests
    {
        string[] games = new[] {
        "X|X|X|X|X|X|X|X|X|X||XX",  // 300 - perfect game
		"X|15|1/|X|9/|X|X|X|X|X||12", // 206
		"X|1/|X|5/|X|3/|X|8/|X|9/||X1/", // 200 - "scottish" 200
		"X|1/|X|5/|X|3/|X|8/|X|--||", // 170
    	};

        string origOutput = "Game: X|X|X|X|X|X|X|X|X|X||XX Score: 300" + Environment.NewLine +
                            "Game: X|15|1/|X|9/|X|X|X|X|X||12 Score: 206" + Environment.NewLine +
                            "Game: X|1/|X|5/|X|3/|X|8/|X|9/||X1/ Score: 200" + Environment.NewLine +
                            "Game: X|1/|X|5/|X|3/|X|8/|X|--|| Score: 170" + Environment.NewLine;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OldProgramCanScore()
        {
            var oldOut = Console.Out;
            var memstream = new MemoryStream();
            var stream = new StreamWriter(memstream);
            stream.AutoFlush = true;
            Console.SetOut(stream);

            var p = new OldProgram();
            p.ScoreGames(games);

            Console.SetOut(oldOut);
            memstream.Seek(0,SeekOrigin.Begin);
            var pgmOutput = new StreamReader(memstream);
            var output = pgmOutput.ReadToEnd();
            Assert.AreEqual(origOutput, output);
        }
    }
}
