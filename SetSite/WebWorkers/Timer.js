var sec = 0;
var timerRunning = true;

function incTimer()
{
    if (timerRunning)
    {
        sec++;
        postMessage(sec);
        setTimeout("incTimer()", 1000);
    }
};

this.onmessage = function (mess)
{
    switch (mess.data.action)
    {
        case "pause": timerRunning = false;
            break;
        case "resume": timerRunning = true;
            setTimeout("incTimer()", 1000);
            break;
        case "setTime": sec = Math.floor(Number(mess.data.time));
            break;
    }
};