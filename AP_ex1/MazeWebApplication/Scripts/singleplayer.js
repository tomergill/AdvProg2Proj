var leftKey = 37;
var upKey = 38;
var rightKey = 39;
var downKey = 40;

var nameOfMaze = null;

function generateButtonClicked(nameEle, rowsEle, colsEle, err) {
    if (!checkGameDetails(nameEle, rowsEle, colsEle, err))
        return;

    $("#loader").show();
    event.preventDefault();

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
        if (timer) {
            clearInterval(timer);
        }
        $("#loader").hide();
        //$("canvas")[0].setAttribute("width", $("canvas")[0].getAttribute("height") * data.Cols / data.Rows);
        //alert($("canvas")[0].getAttribute("width"));

        nameOfMaze = data.Name;
        $("title").html(data.Name);
        $("canvas").mazeBoard("generate", data).focus();
    });
}

function solveButtonClicked(selectEle) {
    if (nameOfMaze == null)
    {
        $("#err").html("<strong>Generate maze before trying to solve, dummy!</strong>").css("visibility", "visible");
        return;
    }

    $("#err").css("visibility", "hidden");
    var algoId = $("#selectAlgo")[0].selectedIndex;
    var url = "../api/Maze";
    url += "/" + nameOfMaze + "?algoId=" + algoId;
    $.getJSON(url, function (data) {
        if (data == null || data == "")
            return;
        console.log(typeof (data));
        console.log(data);
        console.log(data.length);
        $("canvas").mazeBoard("solve", data);
    });
}