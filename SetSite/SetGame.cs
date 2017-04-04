using SetSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetSite
{
    public struct GameState
    {
        public int id { get; set; }
        public List<Card> cards { get; set; }
    }

    public class SetGame
    {
        public List<Card> Deck { get; set; }
        public List<PlayerViewModel> Players { get; set; }
        public List<List<Card>> PotentialSets { get; set; }
        public List<int> StartVotes { get; set; }
        public List<Card> TotalSet { get; set; }
        public GameTimer Timer { get; set; }

        private Random rand;

        public int StateId { get; set; }

        public bool IsPaused { get; set; }

        public SetGame()
        {
            Deck = new List<Card>();
            Players = new List<PlayerViewModel>();
            PotentialSets = new List<List<Card>>();
            StartVotes = new List<int>();
            TotalSet = new List<Card>();
            StateId = 0;
            rand = new Random();
            Timer = new GameTimer();
            IsPaused = true;
        }

        #region Private Methods

        private List<List<Card>> Combination(List<Card> totalSet)
        {
            var combos = new List<List<Card>>();

            for (int i = 0; i < totalSet.Count; i++)
            {
                for (int i2 = i + 1; i2 < totalSet.Count; i2++)
                {
                    for (int i3 = i2 + 1; i3 < totalSet.Count; i3++)
                    {
                        combos.Add(new List<Card>() { totalSet[i], totalSet[i2], totalSet[i3] });
                    }
                }
            }
            //totalSet.ForEach(card, index)
            //{
            //    totalSet.forEach(function (card2, index2)
            //    {
            //        if (index2 > index)
            //        {
            //            totalSet.forEach(function (card3, index3)
            //            {
            //                if (index3 > index2)
            //                {
            //                    combos.push([card, card2, card3]);
            //                }
            //            });
            //        }
            //    });
            //});

            return combos;
        }

        private List<Card> CreateDeck()
        {
            Deck = new List<Card>();
            var cardCount = 0;

            for (var colorNum = 0; colorNum < 3; colorNum++)
            {
                for (var shapeNum = 0; shapeNum < 3; shapeNum++)
                {
                    for (var fillNum = 0; fillNum < 3; fillNum++)
                    {
                        for (var numberNum = 0; numberNum < 3; numberNum++)
                        {
                            var card = new Card() { id = cardCount++ };

                            switch (colorNum)
                            {
                                case 0: card.color = "blue";
                                    break;
                                case 1: card.color = "red";
                                    break;
                                case 2: card.color = "green";
                                    break;
                            }

                            switch (shapeNum)
                            {
                                case 0: card.shape = "circle";
                                    break;
                                case 1: card.shape = "rect";
                                    break;
                                case 2: card.shape = "triangle";
                                    break;
                            }

                            switch (fillNum)
                            {
                                case 0: card.fill = "solid";
                                    break;
                                case 1: card.fill = "striped";
                                    break;
                                case 2: card.fill = "empty";
                                    break;
                            }
                            //card.fill = "solid";

                            card.number = numberNum + 1;

                            Deck.Add(card);
                        }
                    }
                }
            }

            return Deck;
        }

        private void DetermineSets(List<Card> totalSet)
        {
            PotentialSets = new List<List<Card>>();

            var possibleSols = Combination(totalSet);

            foreach (var sol in possibleSols)
            {
                if (IsSet(sol))
                {
                    PotentialSets.Add(sol);
                }
            }
        }

        private void Shuffle()
        {
            TotalSet.ForEach(card => Deck.Add(card));
            TotalSet.Clear();

            for (var i = 0; i < 12; i++)
            {
                var index = (int)Math.Floor(rand.NextDouble() * Deck.Count);
                TotalSet.Add(Deck[index]);
                Deck.RemoveAt(index);
            }
        }

        #endregion

        #region Public Methods

        public bool IsSet(List<Card> set)
        {
            if (set.Count != 3)
            {
                return false;
            }
            else
            {
                var colorSet = set.GroupBy(card => card.color);
                var fillSet = set.GroupBy(card => card.fill);
                var numberSet = set.GroupBy(card => card.number);
                var shapeSet = set.GroupBy(card => card.shape);

                var sameColor = (colorSet.Count() == 1);
                var diffColor = (colorSet.Count() == 3);

                var sameFill = (fillSet.Count() == 1);
                var diffFill = (fillSet.Count() == 3);

                var sameNumber = (numberSet.Count() == 1);
                var diffNumber = (numberSet.Count() == 3);

                var sameShape = (shapeSet.Count() == 1);
                var diffShape = (shapeSet.Count() == 3);

                return (sameColor || diffColor)
                    && (sameFill || diffFill)
                    && (sameNumber || diffNumber)
                    && (sameShape || diffShape);
            }
        }

        public GameState ProcessSet(List<Card> set)
        {
            if (IsSet(set))
            {
                //replace set cards from totalSet
                for (var i = 0; i < 3; i++)
                {
                    var repCard = TotalSet.FirstOrDefault(card => card.id == set[i].id);
                    var repIndex = TotalSet.IndexOf(repCard);
                    //var repIndex = totalSet.indexOf(set[i]);
                    if (Deck.Count > 0)
                    {
                        var deckIndex = (int)Math.Floor(rand.NextDouble() * Deck.Count);
                        TotalSet[repIndex] = Deck[deckIndex];
                        Deck.RemoveAt(deckIndex);
                    }
                    else
                    {
                        TotalSet.RemoveAt(repIndex);
                    }
                }

                DetermineSets(TotalSet);

                while (PotentialSets.Count == 0 && Deck.Count > 0)
                {
                    //can this infinite loop?
                    Shuffle();
                    DetermineSets(TotalSet);
                }

                if (PotentialSets.Count == 0 && Deck.Count == 0)
                {
                    return new GameState() { id = -1, cards = TotalSet };
                }
                else
                {
                    return new GameState() { id = ++StateId, cards = TotalSet };
                }
            }
            else
            {
                return new GameState() { id = StateId, cards = TotalSet };
            }
        }

        public GameState StartGame()
        {
            CreateDeck();           

            while (PotentialSets.Count == 0)
            {
                Shuffle();
                DetermineSets(TotalSet);
            }

            //SetTimer(timerCallback)
            StateId = 1;
            IsPaused = false;

            return new GameState { cards = TotalSet, id = StateId };
        }

        public void Vote(int id)
        {
            if(!StartVotes.Contains(id))
            {
                StartVotes.Add(id);
            }
        }

        #endregion
    }
}