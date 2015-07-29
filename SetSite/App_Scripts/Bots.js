var setApp = (function ()
{
    var botsId = 0;

    var game = new setGame();
    var timer = new gameTimer();
    var setUI = new setUserInterface();


    //private
    function DetermineSet(setElements)
    {
        var selected = [];

        //get card values from UI elements
        [].slice.call(setElements).forEach(function (card)
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
            game.ProcessSet(selected);
            setUI.UpdateScore(selected);//potentially pass setElements
            setUI.AddFoundSet("foundSets", setElements, 5);
            StartNextState();
        }
    };
    function FillOpponentDiv(opp)
    {
        var html = '<div id="Player' + opp.Id + '" class="opponentDiv">' +
                    '<h4>' + (opp.Difficulty ? opp.Difficulty + " " : "") + opp.Name + '</h4>' +
                    '<label>Score:</label> <span class="playerScore"></span>' +
                    '<div class="playerSets"></div>' +
                   '</div>';
        $('#botsDiv').append(html);
    };
    function HandleOppAnswer(mess)
    {
        if (mess.data.stateId == game.currentState().id)
        {
            var set = mess.data.set;
            var id = mess.data.oppId;

            var setElements = [];
            set.forEach(function (card)
            {
                setElements.push(document.getElementById("Card" + card.id));
            });

            game.ProcessSet(set);
            setUI.AddFoundSet('Player' + id + ' .playerSets', setElements, 3);

            var player = game.players.filter(function (player)
            {
                return player.Id == id;
            })[0];

            setUI.UpdateScore(set, player);
            StartNextState();
        }
    };
    function PauseBots()
    {
        game.players.forEach(function (bot)
        {
            if (bot.Worker)
            {
                bot.Worker.postMessage({
                    action: "pause"
                });
            }
        });
    };
    function StartBots()
    {
        game.players.forEach(function (bot)
        {
            if (bot.Worker)
            {
                bot.Worker.postMessage({
                    oppId: bot.Id,
                    difficulty: bot.Difficulty,
                    state: game.currentState()
                });
            }
        });
    };
    function StartNextState()
    {
        setUI.StartNextState(game.currentState());

        if (game.currentState().id == -1)
        {
            timer.Pause();
            PauseBots();
        }
        else
        {
            StartBots();
        }
    };
    function UpdateCompleted(set, oppId)
    {
        var sets, score;
        if (oppId == null)
        {
            completedSets.push(set);
            score = document.getElementById("setCount");
            sets = completedSets;
        }
        else
        {
            var bot = game.players.filter(function (bot)
            {
                return bot.Id == oppId;
            })[0];

            bot.Sets.push(set);
            score = document.getElementById("opp" + bot.Id + "Score");
            sets = bot.Sets;
        }

        score.innerText = sets.length;
    };

    function AddBot(difficulty)
    {
        var worker = new Worker("../WebWorkers/SetBot.js");

        worker.onmessage = HandleOppAnswer;

        var bot = {
            Id: botsId,
            Name: "Bot" + botsId,
            Difficulty: difficulty,
            Sets: [],
            Worker: worker
        };

        game.players.push(bot);

        FillOpponentDiv(bot);

        botsId++;
    };
    function PauseGame()
    {
        timer.Pause();
        setUI.PauseGame();
        PauseBots();
    };
    function ResumeGame()
    {
        setUI.ResumeGame(game.currentState());
        timer.Resume();
        StartBots();
    };
    function StartGame()
    {
        $('.addBotButton').attr('disabled', true);

        game.StartGame();

        setUI.SetSelectedHandler(DetermineSet);
        setUI.DrawBoard(game.currentState());
        setUI.StartGame();

        StartBots();

        timer.Start(0, setUI.TimerHandler);
    };

    return {
        AddBot: AddBot,
        PauseGame: PauseGame,
        ResumeGame: ResumeGame,
        StartGame: StartGame,
    };
})();

$(document).ready(function ()
{
    $('#pauseButton').hide();
    $('#startButton').hide();
});