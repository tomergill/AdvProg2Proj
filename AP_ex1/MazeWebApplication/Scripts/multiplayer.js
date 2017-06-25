var hub = $.connection.gamesHub;

hub.client.startGame = function (maze) {
    $("#mycanvas").mazeBoard("start", maze, hub);
    $("#othercanvas").mazeBoard("other", maze);

};


var ViewModel = function () {
    var self = this; // make 'this' available to subfunctions or closures
    this.games = ko.observableArray(); // enables data binding
    var url = "../api/Maze";
    this.getGames = function () {
        $.getJSON(url).done(function (data) {
            self.games(data);
        });
    }
    // Fetch the initial data
    this.getGames();
    return this;
};

var vm = new ViewModel();

ko.applyBindings(vm); // sets up the data binding

function joinButtonClicked() {

}

function startButtonClicked(mazeName, mazeRows, mazeCols, err) {
    if (!checkGameDetails(nameEle, rowsEle, colsEle, err))
        return;

    $("#loader").show();
    event.preventDefault();

    var name = nameEle.value;
    var rows = parseInt(rowsEle.value);
    var cols = parseInt(colsEle.value);
}