var leftKey = 37;
var upKey = 38;
var rightKey = 39;
var downKey = 40;

var nameOfMaze = null;

function generateButtonClicked(nameEle, rowsEle, colsEle, err) {
    if (!checkGameDetails(nameEle, rowsEle, colsEle, err))
        return;

    var name = nameEle.value;
    var rows = parseInt(rowsEle.value);
    var cols = parseInt(colsEle.value);

    var canvas = document.getElementsByTagName("canvas")[0];

    var url = "../api/Maze";
    url += "/" + name + "?rows=" + rows + "&cols=" + cols;

    var maze;

    $.getJSON(url, function (data) {
        //maze = new MazeViewer(data, canvas);
        if (data == null) {
            $("#err").html("<strong>Sorry, the name of this maze was already taken. Please try another name.</strong>").css("visibility", "visible");
            return;
        }
        nameOfMaze = data.Name;
        $("title").html(data.Name);
        $("canvas").mazeBoard("generate", data).focus();
    });
    //$("body").keydown(function (e) {
    //    switch (e.keyCode) {
    //        case leftKey:

    //            break;
    //        case upKey:
    //            break;
    //        case rightKey:
    //            break;
    //        case downKey:
    //            break;
    //        default:
    //            break;
    //    }
    //});
}