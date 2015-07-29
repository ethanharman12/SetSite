var setApp = (function ()
{
    var gameId = 0;
    var currentState = null;

    var game = new setGame();
    var timer = new gameTimer();
    var setUI = new setUserInterface();

    var gameHub = $.connection.multiplayerGameHub;

    //private
    function DetermineSet(setElements)
    {
        var selected = [];

        //get card values from UI elements
        [].slice.call(setElements).forEach(function (card)
        {
            var id = card.id.substring(4, card.id.length);//Card0-Card81

            var setCard = currentState.cards.filter(function (sCard)
            {
                return sCard.id == id;
            });

            selected.push(setCard[0]);
        });

        if (game.IsSet(selected))
        {
            //playerId might be passed by User to server
            //gameHub.server.sendSet(JSON.stringify({ stateId: currentState.id, set: selected, oppId: playerId }));
            gameHub.server.sendSet({ stateId: currentState.id, set: selected }, gameId);

            setUI.UpdateScore(selected);//potentially pass setElements
            setUI.AddFoundSet("foundSets", setElements, 5);
        }
    };
    function Disconnect(oppId)
    {
        $('#Player' + oppId + ' .playerConnect').toggleClass('connected disconnected glyphicon-ok glyphicon-remove');
        $('#Player' + oppId).css({ opacity: 0.5 });
    };
    function FillOpponentDiv(opp, statsOnly)
    {
        var html = '<div id="Player' + opp.Id + '" class="opponentDiv">' +
                    '<h4><span class="playerConnect connected glyphicon glyphicon-ok"></span> ' + opp.Name + '</h4>' +
                    '<label>Score:</label> <span class="playerScore">' + opp.Sets.length + '</span>' +
                    '<div class="playerVote negativeVote">Vote to Start</div>' +
                    '<div class="playerSets"></div>' +
                   '</div>';
        $('#multiplayerDiv').append(html);

        if (!statsOnly)
        {
            setUI.RedrawSets(opp.Sets, 'Player' + opp.Id + ' .playerSets', 3);
        }
    };
    function HandleOppAnswer(mess)
    {
        if (mess.stateId == currentState.id)
        {
            var set = mess.set;
            var id = mess.oppId;

            var setElements = [];
            set.forEach(function (card)
            {
                setElements.push(document.getElementById("Card" + card.id));
            });

            setUI.AddFoundSet('Player' + id + ' .playerSets', setElements, 3);

            var player = game.players.filter(function (player)
            {
                return player.Id == id;
            })[0];

            setUI.UpdateScore(set, player);
        }
    };
    function PauseGame()
    {
        setUI.PauseGame();
        timer.Pause();
    };
    function ReceiveStartVote(oppId)
    {
        $('#Player' + oppId + ' .playerVote').toggleClass('negativeVote positiveVote');
    };
    function Reconnect(oppId)
    {
        $('#Player' + oppId + ' .playerConnect').toggleClass('disconnected connected glyphicon-ok glyphicon-remove');
        $('#Player' + oppId).css({ opacity: 1 });
    };
    function RedrawSets(sets)
    {
        if (!sets)
        {
            sets = [];
        }

        setUI.RedrawSets(sets, "foundSets", 5);

        $('#setCount')[0].innerText = sets.length;
    };
    function ResumeGame()
    {
        setUI.ResumeGame(currentState);
        timer.Resume();
    };
    function ShowStats(players, seconds)
    {
        players.forEach(function (player)
        {
            FillOpponentDiv(player, true);
        })
        setUI.TimerHandler(timer.SecondsToTime(seconds));
        StartNextState({ id: -1, cards: [] });
    };
    function Spectate(state)
    {
        $('#foundSetsDiv').hide();
        $('#buttonContainer').hide();
        $('#setCount').parent().hide();

        setUI.SetSpectateMode(true);

        if (state.id != 0)
        {
            $('.playerVote').remove();
        }

        StartNextState(state);
    };
    function StartGame()
    {
        setUI.SetSelectedHandler(DetermineSet);
        setUI.StartGame();

        timer.Start(0, setUI.TimerHandler);
    };
    function StartNextState(state)
    {
        //start game
        if (currentState == null)
        {
            StartGame();
        }

        currentState = state;
        setUI.StartNextState(state);

        if (state.id == -1)
        {
            timer.Pause();
        }
    };
    function UpdateTimer(secondsElapsed)
    {
        timer.SetTime(secondsElapsed);
        setUI.TimerHandler(timer.SecondsToTime(secondsElapsed));
    };

    function AddPlayer(player)
    {
        if (!player.Sets)
        {
            player.Sets = [];
        }

        game.players.push(player);

        FillOpponentDiv(player);
    };
    function SendPause()
    {
        gameHub.server.pauseGame(gameId);
    };
    function SendResume()
    {
        gameHub.server.resumeGame(gameId);
    };
    function SendStart()
    {
        gameHub.server.startGame(gameId);
    };
    function SetUpHub()
    {
        gameId = document.getElementById('gameId').textContent;
        $('.friendDiv').on("click", function ()
        {
            $(this).hide();
            var friendId = this.id.substring(6, this.id.length);//friend1

            gameHub.server.sendInvite(gameId, friendId);
        })

        gameHub.client.displayFinishedGame = ShowStats;
        gameHub.client.pauseGame = PauseGame;
        gameHub.client.playerDisconnected = Disconnect;
        gameHub.client.playerJoin = AddPlayer;
        gameHub.client.playerReconnected = Reconnect;
        gameHub.client.resumeGame = ResumeGame;
        gameHub.client.sendSets = RedrawSets;
        gameHub.client.sendStartVote = ReceiveStartVote;
        gameHub.client.setTime = UpdateTimer;
        gameHub.client.spectate = Spectate;
        gameHub.client.startNextState = StartNextState;
        gameHub.client.updateSet = HandleOppAnswer;

        $.connection.hub.start().done(function ()
        {
            gameHub.server.joinGame(gameId);
        });
    };

    return {
        AddPlayer: AddPlayer,
        SendPause: SendPause,
        SendResume: SendResume,
        SendStart: SendStart,
        SetUpHub: SetUpHub
    };
})();

setApp.SetUpHub();

$(document).ready(function ()
{
    $('#pauseButton').hide();
    $('#startButton').hide();

    $('.friendDiv').on("click", function ()
    {
        $(this).hide();
        //SendInvite(gameId, friendId);
    })
})