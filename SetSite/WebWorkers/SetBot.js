var count = 0;
var totalCount = 0;
var multiplier = 0;
var botId = -1;
var initialized = 0;

var state = null;
var counter = null;

this.onmessage = function (mess)
{
    if (mess.data.action == "pause")
    {
        clearTimeout(counter);
    }
    else
    {
        if (initialized == 0)
        {
            switch (mess.data.difficulty)
            {
                case "Easy": multiplier = 20;
                    break;
                case "Hard": multiplier = 5;
                    break;
                case "Medium":
                default: multiplier = 10;
                    break;
            }

            state = { id: -100 };
            botId = mess.data.oppId;
            initialized = 1;
        }

        //game state has changed
        if (state.id != mess.data.state.id)
        {
            clearTimeout(counter);
            state = mess.data.state;
            count = 0;
            totalCount = (Math.random() * multiplier) + 10;
        }

        counter = setTimeout(function () { Solve(); }, 1000);
    }
}

function Solve()
{
    count++;
    if (count > totalCount)
    {
        var sets = DetermineSets(state.cards);
        if (sets.length > 0)
        {
            postMessage({ oppId: botId, set: sets[0], stateId: state.id });
        }
    }
    else
    {
        counter = setTimeout(function () { Solve(); }, 1000);
    }
}

function DetermineSets(totalSet)
{
    solutions = [];

    var possibleSolutions = Combination(totalSet);

    possibleSolutions.forEach(function (sol)
    {
        if (IsSet(sol))
        {
            solutions.push(sol);
        }
    });

    return solutions;
};

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
}

function IsSet(set)
{
    if (set.length != 3)
    {
        return false;
    }
    else
    {
        onlyUnique = function (value, index, self)
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