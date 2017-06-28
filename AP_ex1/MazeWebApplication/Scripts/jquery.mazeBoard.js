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
        var bcgImg = $("#bcgImg")[0];
        //bcgImg.onload = function () {
            /*this._*/context.drawImage(bcgImg, 0, 0, canvas.width, canvas.height);
        finishedBcg = true;
        //}

        var cellWidth = this._cellWidth;
        var cellHeight = this._cellHeight;

        var finishedTiles = false;

        /* Drawing the maze walls */
        var wallImg = $("#wallImg")[0];
        //wallImg.src = "../Resources/wall.png";
        //wallImg.onload = function () {
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
        //}

        /* Drawing the exit */
        var exitImg = $("#exitImg")[0];
        //exitImg.onload = function () {
        while (!finishedBcg || !finishedTiles);
            /*this._*/context.drawImage(exitImg, maze.End.Col * cellWidth, maze.End.Row * cellHeight, cellWidth, cellHeight);
        //}

        var playerImg = $("#playerImg")[0];
        //playerImg.onload = function () {
        while (!finishedBcg || !finishedTiles);
        context.drawImage(playerImg, maze.Start.Col * cellWidth, maze.Start.Row * cellHeight, cellWidth, cellHeight);
        //}

        this._playerImage = playerImg;
        this._cellImage = bcgImg;
        this._playerPos = maze.Start;
        this._exitImage = exitImg;
    }

    play(dRow, dCol) {
        var newRow = this._playerPos.Row + dRow;
        var newCol = this._playerPos.Col + dCol;

        /* out of bounds */
        if (newRow < 0 || newRow >= this._maze.Rows || newCol < 0 || newCol >= this._maze.Cols)
            return false;

        if (this._maze.Maze[newCol + newRow * this._maze.Cols] == '0') {
            var newPos = {
                Row: newRow,
                Col: newCol
            };
            this.playerPos = newPos;
            return true;
        }
        return false; // cell is blocked
    }

    won() { return (this._maze.End.Row == this._playerPos.Row && this._maze.End.Col == this._playerPos.Col); }

    set playerPos(newPos) {
        var context = this._canvas.getContext("2d");
        context.clearRect(this._playerPos.Col * this._cellWidth, this._playerPos.Row * this._cellHeight, this._cellWidth, this._cellHeight);

        context.drawImage(this._cellImage, this._playerPos.Col * this._cellWidth, this._playerPos.Row * this._cellHeight, this._cellWidth, this._cellHeight);

        if (this._playerPos.Row == this._maze.End.Row && this._playerPos.Col == this._maze.End.Col)
            context.drawImage(this._exitImage, this._maze.End.Col * this._cellWidth, this._maze.End.Row * this._cellHeight, this._cellWidth, this._cellHeight);

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

    get mazeName() {
        return this._maze.Name;
    }
}

var timer = null;

(function ($) {
    var mazeObj = null, otherMaze = null;
    var hubObj = null;
    var keyDownFunc = function (e) {
        var isLegalMove = false;
        switch (e.which) {
            case 37: //left
                isLegalMove = mazeObj.play(0, -1);
                break;
            case 38: //up
                isLegalMove = mazeObj.play(-1, 0);
                break;
            case 39: //right
                isLegalMove = mazeObj.play(0, 1);
                break;
            case 40: //down
                isLegalMove = mazeObj.play(1, 0);
                break;
            default:
                break;
        }
        if (mazeObj.won()) {
            alert("You have WON!");
            $(this).off("keydown");
        }
        return isLegalMove;
    };

    var keyDownFuncForMultiplayer = function (e) {
        if (keyDownFunc(e)) { //if this move was legal
            hubObj.server.playMove(mazeObj.mazeName, e.which); //notify server about move
            if (mazeObj.won())
                hubObj.server.closeGame(mazeObj.mazeName);
        }
    }

    var solution = null;
    var indexOfMoveToMake = 0;


    function doAStepInSolution() {
        if (indexOfMoveToMake >= solution.length && !(timer === null)) {
            window.clearInterval(timer);
            timer = null;
            return;
        }
        mazeObj.playerPos = solution[indexOfMoveToMake++];
    }

    $.fn.mazeBoard =
        function (option, mazeOrSolOrDir, hub) {
            if (option == undefined || mazeOrSolOrDir == undefined) {
                if (option == "close" && mazeObj != null && hubObj != null) {
                    hubObj.server.closeGame(mazeObj.mazeName);
                    $(this).off("keydown");
                    mazeObj = null;
                    hubObj = null;
                }
            }
            switch (option) {
                default:
                    break;

                /* singleplayer */
                case "generate":
                    mazeObj = null;
                    $(this).off("keydown");
                    mazeObj = new MazeViewer(mazeOrSolOrDir, this[0]);
                    this.keydown(keyDownFunc);
                    break;
                case "solve":
                    solution = mazeOrSolOrDir;
                    indexOfMoveToMake = 0;
                    $(this).off("keydown");
                    timer = window.setInterval(doAStepInSolution, 750);
                    break;

                /* multiplayer */
                case "start": //your maze board
                    if (hub == undefined)
                        return;
                    hubObj = hub;
                    mazeObj = null;
                    $(this).off("keydown");
                    mazeObj = new MazeViewer(mazeOrSolOrDir, this[0]);
                    this.keydown(keyDownFuncForMultiplayer);
                    break;
                case "other": //start other maze board
                    otherMaze = null;
                    $(this).off("keydown");
                    otherMaze = new MazeViewer(mazeOrSolOrDir, this[0]);
                    break;
                case "play": //other plays
                    if (otherMaze === null)
                        return;
                    switch (mazeOrSolOrDir) {
                        case 37: //left
                            otherMaze.play(0, -1);
                            break;
                        case 38: //up
                            otherMaze.play(-1, 0);
                            break;
                        case 39: //right
                            otherMaze.play(0, 1);
                            break;
                        case 40: //down
                            otherMaze.play(1, 0);
                            break;
                        default:
                            break;
                    }
                    if (otherMaze.won()) {
                        alert("You have lost :(");
                        $("canvas").off("keydown");
                    }
                    break;
            }
            return this;
        };
})(jQuery);