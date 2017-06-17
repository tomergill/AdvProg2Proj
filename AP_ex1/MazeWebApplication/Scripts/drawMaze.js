var rows = null;
var cols = null;
var cellWidth = null;
var cellHeight = null;

function drawMaze(maze, canvasId) {
    var myCanvas = document.getElementById(canvasId);
    var context = mazeCanvas.getContext("2d");
    rows = maze.length;
    cols = maze[0].length;
    cellWidth = mazeCanvas.width / cols;
    cellHeight = mazeCanvas.height / rows;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < cols; j++) {
            if (maze[i][j] == 1) {
                context.fillRect(cellWidth * j, cellHeight * i,
                    cellWidth, cellHeight);
            }
        }
    }
}

function draePlayerOnBoard(row, col)
{
    if (typeof (row) != number || typeof (col) != number)
        return;
    if (row < 0 || row >= rows || col < 0 || col >= cols)
        return;


}