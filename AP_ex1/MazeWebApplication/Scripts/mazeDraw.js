class MazeViewer {
    constructor(maze, canvasId) {
        var canvas = document.getElementById(canvasId);
        var context = canvas.getContext("2d");
        this._rows = maze.length;
        this._cols = maze.length[0];
    }
}