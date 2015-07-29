$(document).ready(function ()
{
    var setUI = new setUserInterface();
    var totalSet = CreateExampleSet();

    function DrawBoard(id)
    {
        var board = document.getElementById(id);
        var cards = "";

        totalSet.forEach(function (card)
        {
            cards += setUI.DrawCard(card);
        });

        board.innerHTML = cards;
    };
    function CreateExampleSet()
    {
        var set = [];
        var i = 0;

        set.push({ id: i++, isNew: false, color: 'red', fill: 'solid', number: 3, shape: 'triangle' });
        set.push({ id: i++, isNew: false, color: 'blue', fill: 'empty', number: 1, shape: 'circle' });
        set.push({ id: i++, isNew: false, color: 'green', fill: 'solid', number: 2, shape: 'rect' });
        set.push({ id: i++, isNew: false, color: 'blue', fill: 'solid', number: 1, shape: 'rect' });
        set.push({ id: i++, isNew: false, color: 'green', fill: 'empty', number: 1, shape: 'circle' });
        set.push({ id: i++, isNew: false, color: 'red', fill: 'solid', number: 2, shape: 'rect' });
        set.push({ id: i++, isNew: false, color: 'blue', fill: 'striped', number: 1, shape: 'triangle' });        
        set.push({ id: i++, isNew: false, color: 'green', fill: 'empty', number: 2, shape: 'circle' });
        set.push({ id: i++, isNew: false, color: 'red', fill: 'striped', number: 2, shape: 'triangle' });
        set.push({ id: i++, isNew: false, color: 'green', fill: 'empty', number: 3, shape: 'circle' });
        set.push({ id: i++, isNew: false, color: 'blue', fill: 'solid', number: 2, shape: 'rect' });        
        set.push({ id: i++, isNew: false, color: 'red', fill: 'striped', number: 3, shape: 'triangle' });

        return set;
    };
    function HighlightSet(id, cards, negative)
    {
        var board = document.getElementById(id);

        cards.forEach(function (card)
        {
            var svg = board.children[card];
            if (negative)
            {
                $(svg).attr('class', function (index, classNames)
                {
                    return classNames + ' cardError';
                });
            }
            else
            {
                $(svg).attr('class', function (index, classNames)
                {
                    return classNames + ' cardSelect';
                });
            }
        });
    }

    DrawBoard("staticBoard");
    DrawBoard("exampleBoardAllSameColorNumber");
    HighlightSet("exampleBoardAllSameColorNumber", [1, 3, 6]);
    DrawBoard("exampleBoardAllSameNumberShapeFill");
    HighlightSet("exampleBoardAllSameNumberShapeFill", [2, 5, 10]);
    DrawBoard("exampleBoardAllSameShapeFillColor");
    HighlightSet("exampleBoardAllSameShapeFillColor", [4, 7, 9]);
    DrawBoard("exampleBoardDifferentNumberFill");
    HighlightSet("exampleBoardDifferentNumberFill", [0, 8, 11], true);
});