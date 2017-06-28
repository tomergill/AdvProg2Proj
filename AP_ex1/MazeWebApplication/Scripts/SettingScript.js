


function SettingsChange(event) {
    var rows = $("#mazeRows").val();
    var cols = $("#mazeCols").val();
    var searchAlgorithm = document.getElementById("selectAlgo").selectedIndex;
    event.preventDefault();

    localStorage.setItem("rows", rows);
    localStorage.setItem("cols", cols);
    localStorage.setItem("SH", searchAlgorithm);

    window.location.replace("singleplayer.html");
    
}

$(document).ready(function () {
    $("#loader").show();
    $("#mazeRows").val() = localStorage.getItem("rows");
    $("#mazeCols").val() = localStorage.getItem("cols");
    document.getElementById("selectAlgo").selectedIndex = localStorage.getItem("SH");
    $("#loader").hide();

}