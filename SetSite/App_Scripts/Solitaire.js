var setApp = (function ()
{
    var game = new setGame();
    var timer = new gameTimer();
    var setUI = new setUserInterface();


    function DetermineSet(selectedSet)
    {
        var selected = [];

        [].slice.call(selectedSet).forEach(function (card)
        {
            var id = card.id.substring(4, card.id.length);//Card0-Card81

            var setCard = game.currentState().cards.filter(function (sCard)
            {
                return sCard.id == id;
            });

            selected.push(setCard[0]);
        });

        if (game.IsSet(selected))
        {
            setUI.AddFoundSet("foundSets", selectedSet, 5);
            setUI.UpdateScore(selected);

            game.ProcessSet(selected);
            setUI.StartNextState(game.currentState());

            DisplayPossibleSets();

            if (game.currentState().id == -1)
            {
                timer.Pause();
            }
        }
    };
    function DisplaySet()
    {
        var sol = game.potentialSets[0];

        sol.forEach(function (card)
        {
            //var svg = $('#Card' + card.id);
            //svg.addClass("preview");
            var svg = document.getElementById('Card' + card.id);
            svg.classList.add('preview');

            setTimeout(function ()
            {
                svg.classList.remove('preview');
            }, 2000);
        });
    };
    function DisplayPossibleSets()
    {
        $('#possibleSetsCount').text(game.potentialSets.length);

        if (game.potentialSets.length > 0)
        {
            $('#showSolButton').attr('disabled', false);
        }
        else
        {
            $('#showSolButton').attr('disabled', true);
        }
    };
    function GenerateSet(isEasy)
    {
        game.StartGame(isEasy);

        setUI.SetSelectedHandler(DetermineSet);
        setUI.DrawBoard(game.currentState());
        setUI.StartGame();

        timer.Start(0, setUI.TimerHandler);

        $("#foundSets")[0].innerHtml = "";
        $('#showSolButton').attr('disabled', true);

        DisplayPossibleSets();
    };
    function PauseGame()
    {
        timer.Pause();
        setUI.PauseGame();
    };
    function ResumeGame()
    {
        timer.Resume();
        setUI.ResumeGame(game.currentState());

        if (game.potentialSets.length > 0)
        {
            $('#showSolButton').attr('disabled', false);
        }
    };
    function TogglePossible()
    {
        var checked = $('#possibleSetsCheck')[0].checked;

        if (typeof (Storage) != 'undefined')
        {
            localStorage.PossibleCheck = checked;
        }
        //save to User Profile

        $('#possibleSetsDisplay').first().toggle(checked);
    };

    return {
        DetermineSet: DetermineSet,
        DisplaySet: DisplaySet,
        GenerateSet: GenerateSet,
        PauseGame: PauseGame,
        ResumeGame: ResumeGame,
        TogglePossible: TogglePossible
    };
})();

$(document).ready(function ()
{
    $('#pauseButton').hide();
    $('#startButton').hide();

    //check user profile first
    if (typeof (Storage) != 'undefined')
    {
        if (localStorage.PossibleCheck != 'undefined')
        {
            $('#possibleSetsCheck')[0].checked = (localStorage.PossibleCheck === 'true');
        }
        else
        {
            $('#possibleSetsCheck')[0].checked = true;
        }
    }

    setApp.TogglePossible();
});