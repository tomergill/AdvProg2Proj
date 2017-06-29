$(document).ready(function () {
    //not logged in
    if (sessionStorage.getItem("id") == undefined)
    {
        alert("You must be logged in to play multiplayer. BE GONE!");
        window.location.replace("/Views/HomePage.html");
    }

    //sets the values as the defaults in the local storage
    document.getElementById("mazeRows").value = localStorage.getItem("rows");
    document.getElementById("mazeCols").value = localStorage.getItem("cols");

    var mazeName = null;

    $("#loader").show();
    var hub = $.connection.gamesHub;


    hub.client.startGame = function (maze) {
        if (maze == null)
            return;
        $("#loader").hide();
        $("#otherLabel").show();
        $("#yourLabel").show();
        $("#othercanvas").mazeBoard("other", maze);
        $("#mycanvas").mazeBoard("start", maze, hub);
        $("title").html(maze.Name);
        mazeName = maze.Name;
        $("canvas").css("border", "3px solid #000000");
        $("label.myLabel").show();
    };

    hub.client.play = function (direction) {
        $("#othercanvas").mazeBoard("play", direction);
    }

    hub.client.closeGame = function (name) {
        if (mazeName === name) {
            alert("Game closed by other player, so... you have WON! (technically)");
            mazeName = null;
            $("canvas").off("keydown");
        }
    }

    $.connection.hub.start().done(function () {
        var ViewModel = function () {
            var self = this; // make 'this' available to subfunctions or closures
            this.games = ko.observableArray(); // enables data binding
            var url = "../api/Maze";
            this.getGames = function () {
                //$.getJSON(url).done(function (data) {
                //    self.games(data);
                //});
                hub.server.getGames().done(function (data) {
                    if (data != null)
                        self.games(data);
                    else
                        $("#err").html("<strong>Error getting list of games from server.</strong>").css("visibility", "visible");
                });
            }
            // Fetch the initial data
            this.getGames();
            return this;
        };

        var vm = new ViewModel();

        ko.applyBindings(vm); // sets up the data binding

        $("#loader").hide();

        /**
         * Joins the requested game
         */
        function joinButtonClicked() {
            if ($("#selectGame")[0].selectedIndex === -1) {
                $("#err").html("<strong>Please select a game to join.</strong>").css("visibility", "visible");
                return;
            }
            $("#err").html("").css("visibility", "hidden");
            var gameName = $("#selectGame")[0].value;
            $("#loader").show();

            hub.server.joinGame(gameName).done(function (data) {
                if (data == null) {
                    $("#err").html("<strong>Game with this name is either full or doesn't exist. Please choose another game.</strong>").css("visibility", "visible");
                    vm.getGames();
                    $("#loader").hide();
                    return;
                }
                if (mazeName != null)
                    hub.server.closeGame(mazeName);
                $("#loader").hide();
                $("#otherLabel").show();
                $("#yourLabel").show();
                $("#othercanvas").mazeBoard("other", data);
                $("#mycanvas").mazeBoard("start", data, hub);
                $("title").html(data.Name);
                mazeName = gameName;
                $("canvas").css("border", "3px solid #000000");
                $("label.myLabel").show();
            });
        }

        /**
         * Start a game with the chosen parameters
         */
        function startButtonClicked() {
            if (!checkGameDetails($("#mazeName")[0], $("#mazeRows")[0], $("#mazeCols")[0], $("#err")[0]))
                return;
            $("#err").html("").css("visibility", "hidden");
            $("#loader").show();

            var name = $("#mazeName")[0].value;
            var rows = parseInt($("#mazeRows")[0].value);
            var cols = parseInt($("#mazeCols")[0].value);

            hub.server.startGame(name, rows, cols).done(function (success) {
                if (success)
                    $("#loader").show();
                else {
                    $("#err").html("<strong>Game with this name is already on. Please choose a different name.</strong>").css("visibility", "visible");
                    return;
                }
                if (mazeName != null)
                    hub.server.closeGame(mazeName);
                mazeName = name;
            });
        }

        $("#joinBtn").click(joinButtonClicked);
        $("#startBtn").click(startButtonClicked);
        $("#rfrshBtn").click(vm.getGames);

    });

    window.onbeforeunload = function (e) {
        alert("onbeforeunload");
        try {
            if (mazeName != null)
                hub.server.closeGame(mazeName);
        } catch (exc) {
            //do nothing
        }
    };
});

/**
 * Updates the server that a user has won/lose
 * @param {boolean} won
 */
function updateUserScore(won) {
    var url = "../api/User/" + sessionStorage.getItem("id").toString() + "/";
    if (won)
        $.post(url + "true");
    else
        $.post(url + "false");
}