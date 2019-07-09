using BowlingScore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        string[] games = new[] {
        "X|X|X|X|X|X|X|X|X|X||XX",  // 300 - perfect game
		"X|15|1/|X|9/|X|X|X|X|X||12", // 206
		"X|1/|X|5/|X|3/|X|8/|X|9/||X", // 200 - "scottish" 200
		"X|1/|X|5/|X|3/|X|8/|X|--||", // 170
    	};

        [SetUp]
        public void Setup()
        {
        }

        [Ignore("Refactor")]
        public void LegacyCanValidateGameString()
        {
            var p = new Program();
            foreach (var g in games)
            {
                var v = p.ValidGameLine(g);
                Assert.IsTrue(v.valid);
            }
        }

        [Test]
        [TestCase("00",ExpectedResult = true)]
        [TestCase("0/", ExpectedResult = true)]
        [TestCase("0/0", ExpectedResult = true)]
        [TestCase("0/X", ExpectedResult = true)]
        [TestCase("X", ExpectedResult = true)]
        [TestCase("XXX", ExpectedResult = true)]
        [TestCase("XX2", ExpectedResult = true)]
        [TestCase("X00", ExpectedResult = true)]
        [TestCase("X0/", ExpectedResult = true)]
        [TestCase("X5/", ExpectedResult = true)]
        [TestCase("X29", ExpectedResult = false)]
        [TestCase("000", ExpectedResult = false)]
        [TestCase("00/", ExpectedResult = false)]
        [TestCase("00X", ExpectedResult = false)]
        [TestCase("92", ExpectedResult = false)]
        [TestCase("/0", ExpectedResult = false)]
        [TestCase("/X", ExpectedResult = false)]
        [TestCase("X/", ExpectedResult = false)]
        [TestCase("X0", ExpectedResult = false)]
        [TestCase("XX", ExpectedResult = false)]
        [TestCase("//", ExpectedResult = false)]
        public bool FrameValidatorKnowsValidStringForFrame(string score)
        {
            return FrameStringValidator.IsValid(score);
        }

        [Test]
        public void CreateBadFrameThrowsException()
        {
            var msg = Assert.Throws<Exception>(() => {
                new Frame("bad");
            }).Message;
            Assert.IsTrue(msg.StartsWith("Invalid Frame String"));

        }

        [Test]
        public void CanCreateFrameFromString()
        {
            var f = new Frame("2/");
            Assert.AreEqual(Frame.ThrowValue.TWO, f.First);
            Assert.AreEqual(Frame.ThrowValue.SPARE, f.Second);
        }

        [Test]
        [TestCase("--", ExpectedResult = 0)]
        [TestCase("01",ExpectedResult = 1)]
        [TestCase("02", ExpectedResult = 2)]
        [TestCase("03", ExpectedResult = 3)]
        [TestCase("04", ExpectedResult = 4)]
        [TestCase("05", ExpectedResult = 5)]
        [TestCase("06", ExpectedResult = 6)]
        [TestCase("07", ExpectedResult = 7)]
        [TestCase("08", ExpectedResult = 8)]
        [TestCase("09", ExpectedResult = 9)]
        public int FrameCanScoreOpen(string frame)
        {
            var f = new Frame(frame);
            (var score, var bonus) = f.Score;
            Assert.AreEqual(0, bonus);
            return score;
        }

        [Test]
        public void FrameCanScoreStrike()
        {
            var f = new Frame("X");
            (var score, var bonus) = f.Score;
            Assert.AreEqual(10, score);
            Assert.AreEqual(2, bonus);
        }

        [Test]
        public void FrameCanScoreSpare()
        {
            var f = new Frame("3/");
            (var score, var bonus) = f.Score;
            Assert.AreEqual(10, score);
            Assert.AreEqual(1, bonus);
        }

        [Test]
        public void FrameCanScoreBonusOpen()
        {
            var f = new Frame("X24");
            (var score, var bonus) = f.Score;
            Assert.AreEqual(16, score);
            Assert.AreEqual(0, bonus);
        }

        [Test]
        public void FrameCanScoreBonusSpare()
        {
            var f = new Frame("X9/");
            (var score, var bonus) = f.Score;
            Assert.AreEqual(20, score);
            Assert.AreEqual(0, bonus);
        }

        [Test]
        public void FrameCanScoreStrikeBonus()
        {
            var f = new Frame("XX3");
            (var score, var bonus) = f.Score;
            Assert.AreEqual(23, score);
            Assert.AreEqual(0, bonus);
        }

        [Test]
        public void FrameCanScoreThreeBagger()
        {
            var f = new Frame("XXX");
            (var score, var bonus) = f.Score;
            Assert.AreEqual(30, score);
            Assert.AreEqual(0, bonus);
        }

        [Test]
        public void InvalidBowlingLineThrowsException()
        {
            var msg = Assert.Throws<Exception>(()=> {
                new BowlingLine("This is bad");
            }).Message;
            Console.WriteLine(msg);
            Assert.True(msg.StartsWith("Invalid"));
        }

        [Test]
        public void CanRecognizeValidBowlingLineString()
        {
            foreach (var g in games)
            {
                var b = new BowlingLine(g);
                Assert.AreEqual(10, b.Frames.Count);
            }
        }

        [Ignore("TBD")]
        public void CanScoreGames()
        {
            var scores = new List<int>();
            foreach (var g in games)
            {
                scores.Add((new BowlingLine(g)).ScoreGame());
            }
            Assert.AreEqual(new double[] { 300, 206, 200, 170 }, scores);
        }
    }
}