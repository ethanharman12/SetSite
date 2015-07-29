var gameTimer = function ()
{
    var timer = null;

    function Pause()
    {
        timer.postMessage({ action: "pause" });
    };
    function Resume()
    {
        timer.postMessage({ action: "resume" });
    };
    function SecondsToTime(secs)
    {
        var seconds = Number(secs);

        var hours = parseInt(seconds / 3600) % 24;
        var minutes = parseInt(seconds / 60) % 60;
        var secs = parseInt(seconds % 60);

        return (hours < 10 ? "0" + hours : hours) + ":"
             + (minutes < 10 ? "0" + minutes : minutes) + ":"
             + (secs < 10 ? "0" + secs : secs);
    };
    function SetTime(time)
    {
        if (timer)
        {
            timer.postMessage({ action: "setTime", time: time });
        }
    };
    function Start(currentTime, callback)
    {
        if (typeof (Worker) != undefined)
        {
            if (timer == null)
            {
                timer = new Worker("../../WebWorkers/Timer.js");

                timer.postMessage({ action: "setTime", time: currentTime });
                timer.postMessage({ action: "resume" });

                timer.onmessage = function (results)
                {
                    callback(SecondsToTime(results.data));
                };
            }
        }
    }

    return {
        Pause: Pause,
        Resume: Resume,
        SecondsToTime: SecondsToTime,
        SetTime: SetTime,
        Start: Start
    };
};