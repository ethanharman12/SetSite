var setGame = function ()
{
    var deck = [];
    var players = [];
    var potentialSets = [];
    var totalSet = [];

    var stateId = 0;
    var currentState = null;

    //private
    function Combination(totalSet)
    {
        var combos = [];

        totalSet.forEach(function (card, index)
        {
            totalSet.forEach(function (card2, index2)
            {
                if (index2 > index)
                {
                    totalSet.forEach(function (card3, index3)
                    {
                        if (index3 > index2)
                        {
                            combos.push([card, card2, card3]);
                        }
                    });
                }
            });
        });

        return combos;
    };
    function CreateDeck(isEasy)
    {
        var cardCount = 0;

        for (var colorNum = 0; colorNum < 3; colorNum++)
        {
            for (var shapeNum = 0; shapeNum < 3; shapeNum++)
            {
                for (var numberNum = 0; numberNum < 3; numberNum++)
                {
                    for (var fillNum = 0; fillNum < (isEasy ? 1 : 3) ; fillNum++)
                    {
                        var card = {
                            id: cardCount,
                            isNew: true
                        };
                        cardCount++;

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

                        card.number = numberNum + 1;

                        deck.push(card);
                    }
                }
            }
        }
    };
    function DetermineSets(totalSet)
    {
        potentialSets.length = 0;

        var possibleSolutions = Combination(totalSet);

        possibleSolutions.forEach(function (sol)
        {
            if (IsSet(sol))
            {
                potentialSets.push(sol);
            }
        });
    };
    function Shuffle()
    {
        totalSet.forEach(function (card)
        {
            deck.push(card);
        });

        totalSet.length = 0;

        for (var i = 0; i < 12; i++)
        {
            var index = Math.floor(Math.random() * deck.length);
            deck[index].isNew = true;
            totalSet.push(deck[index]);
            deck.splice(index, 1);
        }
    };

    function IsSet(set)
    {
        if (set.length != 3)
        {
            return false;
        }
        else
        {
            var onlyUnique = function (value, index, self)
            {
                return self.indexOf(value) === index;
            };

            var colorSet = set.map(function (card) { return card.color; }).filter(onlyUnique);
            var fillSet = set.map(function (card) { return card.fill; }).filter(onlyUnique);
            var numberSet = set.map(function (card) { return card.number; }).filter(onlyUnique);
            var shapeSet = set.map(function (card) { return card.shape; }).filter(onlyUnique);

            var sameColor = (colorSet.length == 1);
            var diffColor = (colorSet.length == 3);

            var sameFill = (fillSet.length == 1);
            var diffFill = (fillSet.length == 3);

            var sameNumber = (numberSet.length == 1);
            var diffNumber = (numberSet.length == 3);

            var sameShape = (shapeSet.length == 1);
            var diffShape = (shapeSet.length == 3);

            return (sameColor || diffColor)
                && (sameFill || diffFill)
                && (sameNumber || diffNumber)
                && (sameShape || diffShape);
        }
    };
    function ProcessSet(set)
    {
        if (IsSet(set))
        {
            //replace set cards from totalSet
            for (var i = 0; i < 3; i++)
            {
                var repCard = totalSet.filter(function (card) { return card.id == set[i].id })[0];
                var repIndex = totalSet.indexOf(repCard);
                //var repIndex = totalSet.indexOf(set[i]);
                if (deck.length > 0)
                {
                    var deckIndex = Math.floor(Math.random() * deck.length);
                    deck[deckIndex].isNew = true;
                    totalSet[repIndex] = deck[deckIndex];
                    deck.splice(deckIndex, 1);
                }
                else
                {
                    totalSet.splice(repIndex, 1);
                }
            }

            DetermineSets(totalSet);

            while (potentialSets.length == 0 && deck.length > 0)
            {
                //can this infinite loop?
                Shuffle();
                DetermineSets(totalSet);
            }

            if (potentialSets.length == 0 && deck.length == 0)
            {
                //no more possible sets: end game
                currentState = { id: -1, cards: totalSet };
            }
            else
            {
                currentState = { id: ++stateId, cards: totalSet };
            }
        }
        else
        {
            currentState = { id: stateId, cards: totalSet };
        }
    };
    function StartGame(isEasy)
    {
        CreateDeck(isEasy);

        while (potentialSets.length == 0)
        {
            Shuffle();
            DetermineSets(totalSet);
        }

        currentState = { cards: totalSet, id: stateId };
    }

    return {
        //arrays
        players: players,
        potentialSets: potentialSets,

        //getters
        currentState: function () { return currentState; },

        //methods
        IsSet: IsSet,
        ProcessSet: ProcessSet,
        StartGame: StartGame
    };
};