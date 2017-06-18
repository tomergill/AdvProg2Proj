function generateButtonClicked(nameEle, rowsEle, colsEle, err) {
    if (!checkGameDetails(nameEle, rowsEle, colsEle, err))
        return;

    var name = nameEle.value;
    var rows = parseInt(rowsEle.value);
    var cols = parseInt(colsEle.value);

    var url = "api/Maze";
    url += "/" + name;

    var maze = $.post(url, )
}