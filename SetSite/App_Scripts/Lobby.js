var gameHub = $.connection.multiplayerGameHub;

function AddGame(id)
{
    var html = "<div><a href='Multiplayer/" + id + "'>Game " + id + "</a></div>";

    $('#runningGamesDiv').append(html);
}

$(document).ready(function ()
{
    //gameHub.client.receiveInvite = function (gameId)
    //{
    //    alert(gameId);
    //};
    //gameHub.client.gameAdded = AddGame;
    gameHub.client.gameAdded = function (id)
    {
        AddGame(id);
    };
    $.connection.hub.start().done(function ()
    {
        gameHub.server.getCurrentGames().done(function (ids)
        {
            ids.forEach(function (id)
            {
                AddGame(id);
            });
        });

        $('#createGameButton').on("click", function ()
        {
            gameHub.server.createGame().done(function (id)
            {
                window.location.href = "/Home/Multiplayer/" + id;
            });
        });
    });
});