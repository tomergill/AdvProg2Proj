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

        this._playerImage = playerImg;
        this._cellImage = bcgImg;
        this._playerPos = maze.Start;
    }

    play(dRow, dCol) {
        var newRow = this._playerPos.Row + dRow;
        var newCol = this._playerPos.Col + dCol;

        if (newRow < 0 || newRow >= this._maze.Rows || newCol < 0 || newCol >= this._maze.Cols)
            return false;

        if (this._maze.Maze[newCol + newRow * this._maze.Cols] == '0')
        {
            var newPos = {
                Row: newRow,
                Col: newCol
            };
            this.playerPos = newPos;
            return (this._maze.End.Row == this._playerPos.Row && this._maze.End.Col == this._playerPos.Col);
        }
        return false;
    }

    set playerPos(newPos) {
        var context = this._canvas.getContext("2d");
        context.clearRect(this._playerPos.Col * this._cellWidth, this._playerPos.Row * this._cellHeight, this._cellWidth, this._cellHeight);
        context.drawImage(this._cellImage, this._playerPos.Col * this._cellWidth, this._playerPos.Row * this._cellHeight, this._cellWidth, this._cellHeight);
        this._playerPos = newPos;
        context.drawImage(this._playerImage, this._playerPos.Col * this._cellWidth, this._playerPos.Row * this._cellHeight, this._cellWidth, this._cellHeight);
    }

    get playerPos() {
        return { Row: this._playerPos.Row, Col: this._playerPos.Col };
    }

    get cellImg() {
        return this._cellImage;
    }

    get playerImg() {
        return this._playerImage;
    }

    get cellWidth() {
        return this._cellWidth;
    }

    get cellHeight() {
        return this._cellHeight;
    }
}

(function ($) {
    var mazeObj;

    

    $.fn.mazeBoard =
        function (option, mazeOrSol) {
            switch (option) {
                default:
                    break;
                case "generate":
                    var keyDownFunc = function (e) {
                        var won = false;
                        switch (e.which) {
                            case 37: //left
                                won = mazeObj.play(0, -1);
                                break;
                            case 38: //up
                                won = mazeObj.play(-1, 0);
                                break;
                            case 39: //right
                                won = mazeObj.play(0, 1);
                                break;
                            case 40: //down
                                won = mazeObj.play(1, 0);
                                break;
                            default:
                                break;
                        }
                        if (won) {
                            alert("You have WON!");
                            $(this).off("keydown");
                        }
                    };
                    mazeObj = new MazeViewer(mazeOrSol, this[0]);
                    this.keydown(keyDownFunc);
                    break;
            }
            return this;
        };
    })(jQuery);