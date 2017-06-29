var leftKey = 37;
var upKey = 38;
var rightKey = 39;
var downKey = 40;

var nameOfMaze = null;

$(document).ready(function () {
    document.getElementById("mazeRows").value = localStorage.getItem("rows");
    document.getElementById("mazeCols").value = localStorage.getItem("cols");
    document.getElementById("selectAlgo").selectedIndex = localStorage.getItem("SH");
});

/**
 * Generates a maze with the requested parameters
 * @param {HTMLInputElement} nameEle element containing the name
 * @param {HTMLInputElement} rowsEle element containing the rows
 * @param {HTMLInputElement} colsEle element containing the cols
 * @param {HTMLLabelElement} err label for error
 */
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
        $("#loader").hide();
        if (data == null) {
            $("#err").html("<strong>Sorry, the name of this maze was already taken. Please try another name.</strong>").css("visibility", "visible");
            return;
        }
        $("#err").html("").css("visibility", "hidden");
        if (timer) {
            clearInterval(timer);
        }
        //$("#loader").hide();
        //$("canvas")[0].setAttribute("width", $("canvas")[0].getAttribute("height") * data.Cols / data.Rows);
        //alert($("canvas")[0].getAttribute("width"));

        nameOfMaze = data.Name;
        $("title").html(data.Name);

        $("canvas").mazeBoard("generate", data).focus();
        $("canvas").css("border", "3px solid #000000");
    });
}

/**
 * Gets a solution for the maze and displays it.
 * @param {HTMLSelectElement} selectEle element selecting algo
 */
function solveButtonClicked(selectEle) {
    if (nameOfMaze == null)
    {
        $("#err").html("<strong>Generate maze before trying to solve, dummy!</strong>").css("visibility", "visible");
        return;
    }
    $("#loader").show();
    $("#err").css("visibility", "hidden");
    var algoId = $("#selectAlgo")[0].selectedIndex;
    var url = "../api/Maze";
    url += "/" + nameOfMaze + "?algoId=" + algoId;
    $.getJSON(url, function (data) {
        $("#loader").hide();
        if (data == null || data == "") {
            $("#err").html("<strong>Connection error.</strong>").css("visibility", "visible");
            return;
        }
        //console.log(typeof (data));
        //console.log(data);
        //console.log(data.length);
        $("#loader").hide();
        $("#err").html("").css("visibility", "hidden");
        $("canvas").mazeBoard("solve", data);
    });
}