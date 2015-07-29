/// <reference path="../../setsite/app_scripts/timer.js" />
/// <reference path="../../setsite/app_scripts/setgame.js" />
/// <reference path="../../setsite/app_scripts/setui.js" />
/// <reference path="../../setsite/app_scripts/solitaire.js" />

QUnit.test("will determine that set is a Set", function (assert)
{
    expect(1);

    var set = [];
    set.push({ id: 1, isNew: false, color: 'red', fill: 'solid', number: 3, shape: 'triangle' });
    set.push({ id: 2, isNew: false, color: 'blue', fill: 'empty', number: 1, shape: 'circle' });
    set.push({ id: 3, isNew: false, color: 'green', fill: 'striped', number: 2, shape: 'rect' });

    var game = new setGame();
    var res = game.IsSet(set);
    assert.equal(res, true, "should be true");
});
QUnit.test("will determine that set isn't a Set", function (assert)
{
    expect(1);

    var set = [];
    set.push({ id: 1, isNew: false, color: 'red', fill: 'solid', number: 3, shape: 'triangle' });
    set.push({ id: 2, isNew: false, color: 'blue', fill: 'empty', number: 1, shape: 'circle' });
    set.push({ id: 3, isNew: false, color: 'green', fill: 'solid', number: 2, shape: 'rect' });

    var game = new setGame();
    var res = game.IsSet(set);
    assert.equal(res, false, "should be false");
});