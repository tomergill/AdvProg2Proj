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
        maze = new MazeViewer(data, canvas);
    });
}