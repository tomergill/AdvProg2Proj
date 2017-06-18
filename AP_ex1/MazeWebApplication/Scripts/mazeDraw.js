class MazeViewer {
    constructor(maze, canvas) {
        this._maze = maze;
        /*this._*/var context = canvas.getContext("2d");
        this._cellWidth = canvas.width / maze.Cols;
        this._cellHeight = canvas.height / maze.Rows;
        this._canvas = canvas;

        /**************************/
        //var context = new CanvasRenderingContext2D();
        //var canvas = new HTMLCanvasElement();
        /**************************/

        /* Drawing the maze's background */
        var bcgImg = new Image(canvas.width, canvas.height);
        bcgImg.src = "../Resources/background.jpg";
        bcgImg.onload = function () {
            /*this._*/context.drawImage(bcgImg, 0, 0);
        }

        /* Drawing the maze walls */
        var wallImg = new Image(this._cellWidth, this._cellHeight);
        wallImg.src = "../Resources/wall.png";
        wallImg.onload = function () {
            for (var i = 0; i < maze.Rows; i++) {
                for (var j = 0; j < maze.Cols; j++) {
                    /*this._*/context.drawImage(wallImg, j * this._cellWidth, i * this._cellHeight);
                }
            }
        }

        /* Drawing the exit */
        var exitImg = new Image(this._cellWidth, this._cellHeight);
        exitImg.src = "../Resources/exit.png";
        exitImg.onload = function () {
            /*this._*/context.drawImage(wallImg, maze.End.Col * this._cellWidth, maze.End.Row * this._cellHeight);
        }
    }
}