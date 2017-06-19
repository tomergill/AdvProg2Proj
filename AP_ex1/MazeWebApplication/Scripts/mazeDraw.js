class MazeViewer {
    constructor(maze, canvas) {
        this._maze = maze;
        /*this._*/var context = canvas.getContext("2d");
        this._cellWidth = canvas.width / maze.Cols;
        this._cellHeight = canvas.height / maze.Rows;
        this._canvas = canvas;
        var repr = maze.Maze;

        /**************************/
        //var context = new CanvasRenderingContext2D();
        //var canvas = new HTMLCanvasElement();
        /**************************/

        var finishedBcg = false;

        /* Drawing the maze's background */
        var bcgImg = new Image(canvas.width, canvas.height);
        bcgImg.src = "../Resources/background.jpg";
        bcgImg.onload = function () {
            /*this._*/context.drawImage(bcgImg, 0, 0, canvas.width, canvas.height);
            finishedBcg = true;
        }

        var cellWidth = this._cellWidth;
        var cellHeight = this._cellHeight;

        var finishedTiles = false;

        /* Drawing the maze walls */
        var wallImg = new Image(this._cellWidth, this._cellHeight);
        wallImg.src = "../Resources/wall.png";
        wallImg.onload = function () {
            while (!finishedBcg);
            for (var i = 0; i < maze.Rows; i++) {
                for (var j = 0; j < maze.Cols; j++) {
                    if (repr[i * maze.Cols + j] == 1)
                    /*this._*/context.drawImage(wallImg, j * cellWidth, i * cellHeight, cellWidth, cellHeight);
                    else
                        context.drawImage(bcgImg, j * cellWidth, i * cellHeight, cellWidth, cellHeight);
                }
            }
            finishedTiles = true;
        }

        /* Drawing the exit */
        var exitImg = new Image(this._cellWidth, this._cellHeight);
        exitImg.src = "../Resources/exit.png";
        exitImg.onload = function () {
            while (!finishedBcg || !finishedTiles);
            /*this._*/context.drawImage(exitImg, maze.End.Col * cellWidth, maze.End.Row * cellHeight, cellWidth, cellHeight);
        }

        var playerImg = new Image(cellWidth, cellHeight);
        playerImg.src = "../Resources/player.png";
        playerImg.onload = function () {
            while (!finishedBcg || !finishedTiles);
            context.drawImage(playerImg, maze.Start.Col * cellWidth, maze.Start.Row * cellHeight, cellWidth, cellHeight);
        }

        this._cellImage = bcgImg;
    }
}