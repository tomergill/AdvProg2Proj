/**
 * Saves the settings to the local storage
 * @param {any} event
 */
function SettingsChange(event) {
    event.preventDefault();
    
    localStorage.setItem("rows", $("#mazeRows").val());
    localStorage.setItem("cols", $("#mazeCols").val());
    localStorage.setItem("SH", document.getElementById("selectAlgo").selectedIndex);

    window.location.replace("HomePage.html");
    
}

//sets the inputs values to the values saved in the local storage
$(document).ready(function () {
    $("#loader").show();
    document.getElementById("mazeRows").value = localStorage.getItem("rows");
    document.getElementById("mazeCols").value = localStorage.getItem("cols");
    document.getElementById("selectAlgo").selectedIndex = localStorage.getItem("SH");
    $("#loader").hide();

});