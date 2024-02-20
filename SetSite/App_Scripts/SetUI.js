var setUserInterface = function ()
{
    var setHandler = null;
    var spectateMode = false;

    var completedSets = [];

    //private
    function ToggleSelect()
    {
        if ($(this).attr('class').indexOf("cardSelect") > 0)
        {
            $(this).attr('class', function (index, classNames)
            {
                return classNames.replace(' cardSelect', '');
            });
        }
        else
        {
            $(this).attr('class', function (index, classNames)
            {
                return classNames + ' cardSelect';
            });
        }

        var setElements = document.getElementsByClassName("cardSelect");
        if (setElements.length == 3)
        {
            setHandler(setElements);
        }
    };
    function WriteGradients()
    {
        var cardHtml = "<svg><defs>";
        ["blue", "red", "green"].forEach(function (color)
        {
            cardHtml += "<linearGradient id='Gradient" + color + "' x1='0%' y1='0%' x2='100%' y2='100%'>" +
                            "<stop offset='10%' stop-color='" + color + "' />" +
                            "<stop offset='20%' stop-color='#FFF' />" +
                            "<stop offset='30%' stop-color='" + color + "' />" +
                            "<stop offset='40%' stop-color='#FFF' />" +
                            "<stop offset='50%' stop-color='" + color + "' />" +
                            "<stop offset='60%' stop-color='#FFF' />" +
                            "<stop offset='70%' stop-color='" + color + "' />" +
                            "<stop offset='80%' stop-color='#FFF' />" +
                            "<stop offset='90%' stop-color='" + color + "' />" +
                        "</linearGradient>";
        });
        cardHtml += "</svg></defs>";

        var div = document.createElement('div');
        div.innerHTML = cardHtml;
        document.body.appendChild(div);
    };

    function AddFoundSet(foundDiv, setElements, divsToKeep)
    {
        var selectedHTML = "";

        var setEles = [].slice.call(setElements);

        for (var i = 0; i < 3; i++)
        {
            $(setEles[i]).attr('class', function (index, classNames)
            {
                return classNames.replace(' cardSelect', '');
            });
            $(setEles[i]).attr('class', function (index, classNames)
            {
                return classNames + ' exampleCard';
            });
            $("#" + setEles[i].id).off("click", ToggleSelect);
            selectedHTML += $('<div>').append($(setEles[i]).clone()).html();
        }

        $("#" + foundDiv).prepend("<div><div class='foundRow'>" + selectedHTML + "</div></div>");

        Rescale($('#' + foundDiv + ' .foundRow:first')[0]);

        $("#" + foundDiv + " > div:nth-of-type(" + (divsToKeep + 1) + ")").remove();
    };
    function ClearBoard()
    {
        var board = document.getElementById("setBoard");
        board.innerHTML = "";
    };
    function DrawBoard(state)
    {
        var board = document.getElementById("setBoard");
        var cards = "";

        state.cards.forEach(function (card)
        {
            cards += DrawCard(card);
        });

        board.innerHTML = cards;

        if (!spectateMode && state.id != -1)
        {
            state.cards.forEach(function (card)
            {
                var cardEle = document.getElementById("Card" + card.id);
                cardEle.addEventListener("click", ToggleSelect);

                if (card.isNew)
                {
                    card.isNew = false;
                    window.getComputedStyle(cardEle).opacity;
                    cardEle.style.opacity = 1;
                }
            });
        }
        else
        {
            state.cards.forEach(function (card)
            {
                $('#Card' + card.id).attr('class', function (index, classNames)
                {
                    return classNames + ' exampleCard';
                });
            });
        }
    };
    function DrawCard(card)
    {
        var cardHtml = "<svg id='Card" + card.id + "' class='setCard' style='opacity:" + (card.isNew ? "0" : "1") + "'>";

        var points = [];

        switch (card.number)
        {
            case 2:
                points.push({ x: 35, y: 35 });
                points.push({ x: 65, y: 65 });
                break;
            case 3:
                points.push({ x: 25, y: 25 });
                points.push({ x: 75, y: 75 });
            case 1:
                points.push({ x: 50, y: 50 });
                break;
        }

        points.forEach(function (pt)
        {
            switch (card.shape)
            {
                case "circle":
                    cardHtml += "<circle cx='" + pt.x + "' cy='" + pt.y + "' r='10' ";

                    //cardHtml += "stroke='" + card.color + "' stroke-width='1' fill='" + card.color + "' />";
                    break;
                case "rect":
                    cardHtml += "<rect x='" + (pt.x - 10) + "' y='" + (pt.y - 10) + "' height='20' width='20' ";
                    //cardHtml += "style='fill:" + card.color + ";stroke:" + card.color + ";' />";
                    break;
                case "triangle":
                    cardHtml += "<polygon points='"
                              + pt.x + "," + (pt.y - 10) + " "
                              + (pt.x - 10) + "," + (pt.y + 10) + " "
                              + (pt.x + 10) + "," + (pt.y + 10) + "' ";
                    //cardHtml += "style='fill:" + card.color + ";stroke:" + card.color + "' />";
                    break;
            }

            switch (card.fill)
            {
                case "empty":
                    cardHtml += "style='fill:white;stroke:" + card.color + "' />";
                    break;
                case "solid":
                    cardHtml += "style='fill:" + card.color + ";stroke:black' />";
                    break;
                case "striped":
                    cardHtml += "style='fill:url(#Gradient" + card.color + ");stroke:black' />";
                    break;
            }

        });

        cardHtml += "</svg>";
        return cardHtml
    };
    function EndGame(state)
    {
        $('#pauseButton')[0].disabled = true;

        var board = document.getElementById("setBoard");
        board.innerHTML += "<div class='gameOverBanner'>Game Over</div>";
    };
    function PauseGame()
    {
        ClearBoard();

        var board = document.getElementById("setBoard");
        board.innerHTML += "<div class='pauseBanner'>Paused</div>";

        $('#pauseButton')[0].disabled = true;
        $('#startButton')[0].disabled = false;
    };
    function RedrawSets(sets, foundDiv, divsToKeep)
    {
        sets.reverse();
        for (var i = 0; i < divsToKeep; i++)
        {
            if (sets.length > i)
            {
                var setElements = [];
                sets[i].forEach(function (card)
                {
                    setElements.push(DrawCard(card));
                });
                AddFoundSet(foundDiv, setElements, divsToKeep);
            }
        }
    };
    function Rescale(elem)
    {
        var width = elem.getBoundingClientRect().width;
        var height = elem.getBoundingClientRect().height;
        $(elem).css('width', (width / .4) + 'px');
        $(elem).css('height', (height / .4) + 'px');
        $(elem).parent().css('width', width + 'px');
        $(elem).parent().css('height', height + 'px');
    };
    function ResumeGame(state)
    {
        DrawBoard(state);

        $('#pauseButton')[0].disabled = false;
        $('#startButton')[0].disabled = true;
    };
    function SetSelectedHandler(callback)
    {
        setHandler = callback;
    };
    function SetSpectateMode(mode)
    {
        spectateMode = mode;
    };
    function StartGame()
    {
        $('.addPlayerButton').attr('disabled', true);

        $('.generateButton').hide();

        $('.playerVote').remove();

        var pauseButton = $('#pauseButton');
        pauseButton.show();
        pauseButton[0].disabled = false;

        var resumeButton = $('#startButton');
        resumeButton.show();
        resumeButton[0].disabled = true;

        WriteGradients();
    };
    function StartNextState(state)
    {
        DrawBoard(state);

        if (state.id == -1)
        {
            EndGame(state);
        }
    };
    function TimerHandler(result)
    {
        var time = document.getElementById("timePlayed");
        time.innerText = result;
    };
    function UpdateScore(set, player)
    {
        var sets, score;
        if (!player)
        {
            completedSets.push(set);
            score = document.getElementById("setCount");
            sets = completedSets;
        }
        else
        {
            player.Sets.push(set);
            score = $("#Player" + player.Id + " .playerScore")[0];
            sets = player.Sets;
        }

        score.innerText = sets.length;
    };

    return {
        AddFoundSet: AddFoundSet,
        ClearBoard: ClearBoard,
        DrawBoard: DrawBoard,
        DrawCard: DrawCard,
        PauseGame: PauseGame,
        RedrawSets: RedrawSets,
        Rescale: Rescale,
        ResumeGame: ResumeGame,
        SetSelectedHandler: SetSelectedHandler,
        SetSpectateMode: SetSpectateMode,
        StartGame: StartGame,
        StartNextState: StartNextState,
        TimerHandler: TimerHandler,
        UpdateScore: UpdateScore,
        WriteGradients: WriteGradients
    };
};