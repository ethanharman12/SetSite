onmessage = function (e)
{
    postMessage(DetermineSets(e.data.sets));
};

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
    //foreach (var sol in possibleSolutions)
    //{
    //    if (IsSet(sol))
    //    {
    //        solutions.Add(sol);
    //        if (findFirst)
    //        {
    //            return solutions;
    //        }
    //    }
    //}
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

    //totalSet.forEach(function(card, index)
    //{
    //    totalSet.filter(function(card2f, index2f)
    //    {
    //        return index2f > index;
    //    }).forEach(function(card2, index2)
    //    {
    //        totalSet.filter(function(card3f, index3f)
    //        {
    //            return index3f > index2 && index3f > index;
    //        }).forEach(function(card3, index3)
    //        {
    //            combos.push([card, card2, card3]);
    //        });
    //    });
    //});


    //foreach (var card in totalSet)
    //{
    //    foreach (var card2 in set.Skip(set.IndexOf(card) + 1).Where(crd => crd.Id != card.Id))
    //    {
    //        foreach (var card3 in set.Skip(set.IndexOf(card2) + 1).Where(crd => crd.Id != card.Id && crd.Id != card2.Id))
    //        {
    //            List<Card> combo = new List<Card>();
    //            combo.Add(card);
    //            combo.Add(card2);
    //            combo.Add(card3);

    //            combos.Add(combo);
    //        }
    //    }
    //}
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