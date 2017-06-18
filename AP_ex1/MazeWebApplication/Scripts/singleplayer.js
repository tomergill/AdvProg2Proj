function generateButtonClicked(nameEle, rowsEle, colsEle, err, canvas) {
    if (!checkGameDetails(nameEle, rowsEle, colsEle, err))
        return;

    var name = nameEle.value;
    var rows = parseInt(rowsEle.value);
    var cols = parseInt(colsEle.value);

    var url = "../api/Maze";
    url += "/" + name + "?rows=" + rows + "&cols=" + cols;

    var maze;

    $.getJSON(url, function (data) {
        maze = new MazeViewer(data, canvas);
    });
}