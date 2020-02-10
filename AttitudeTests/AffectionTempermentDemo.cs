using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateEngine;

namespace AttitudeTests
{
    [TestClass]
    public class AffectionTempermentDemo
    {
        [TestMethod]
        public void SenpaiWillNeverLoveMe()
        {
            var senpai = AffectionateActor.Spawn();

            Assert.AreEqual(AffectionTemper.Unaware, senpai.Affection.Status);

            senpai.IntroductionHappens();

            Assert.AreEqual(AffectionTemper.Indifferent, senpai.Affection.Status);

            senpai.AffectionIncreases();

            Assert.AreEqual(AffectionTemper.Amicable, senpai.Affection.Status);

            senpai.AffectionDecreases();
            senpai.AffectionDecreases();

            Assert.AreEqual(AffectionTemper.Disdainful, senpai.Affection.Status);

            senpai.AffectionDecreases();

            Assert.AreEqual(AffectionTemper.Hate, senpai.Affection.Status);
        }

        public enum AffectionTemper
        {
            Unaware,
            Indifferent,
            Disdainful,
            Amicable,
            Love,
            Hate
        }

        public class AffectionateActor
        {           
            public MappedState<AffectionTemper> Affection { get; private set; }

            private bool hasBeenIntroduced;

            private int affectionLevel = 0;

            public bool HasBeenIntroduced() => hasBeenIntroduced;

            public static AffectionateActor Spawn()
            {
                return new AffectionateActor();
            }

            public void LifeUpdate()
            {
                Affection.Update();
            }

            public void IntroductionHappens()
            {
                this.hasBeenIntroduced = true;
                Affection.Update();
            }

            public void AffectionIncreases()
            {
                this.affectionLevel++;
                Affection.Update();
            }

            public void AffectionDecreases()
            {
                this.affectionLevel--;
                Affection.Update();
            }

            private AffectionateActor()
            {
                this.Affection = AffectionateStateEngine();
            }

            private MappedState<AffectionTemper> AffectionateStateEngine()
            {
                return MappedState<AffectionTemper>.StartsAs(AffectionTemper.Unaware)
                    .PathOf(
                        AffectionTemper.Unaware
                            .LeadsTo(AffectionTemper.Indifferent)
                            .When(this.HasBeenIntroduced))


                    .PathOf(
                        AffectionTemper.Indifferent
                            .LeadsTo(AffectionTemper.Disdainful)
                            .When(() => this.affectionLevel < 0 && this.affectionLevel > -2))
                    .PathOf(
                        AffectionTemper.Indifferent
                            .LeadsTo(AffectionTemper.Amicable)
                            .When(() => this.affectionLevel > 0 && this.affectionLevel < 2))


                    .PathOf(
                        AffectionTemper.Amicable
                            .LeadsTo(AffectionTemper.Love)
                            .When(() => this.affectionLevel >= 2))
                    .PathOf(
                        AffectionTemper.Amicable
                            .LeadsTo(AffectionTemper.Disdainful)
                            .When(() => this.affectionLevel < 0))


                    .PathOf(
                        AffectionTemper.Disdainful
                            .LeadsTo(AffectionTemper.Hate)
                            .When(() => this.affectionLevel <= -2));
            }
        }
    }
}