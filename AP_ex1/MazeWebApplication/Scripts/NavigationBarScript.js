﻿
$(document).ready(function loadBar() {
    $("#navbar").load("NavigationBar.html", function () {
        if (sessionStorage.getItem("id")) { //user is logged in
            document.getElementById("register").textContent = "Hello " + sessionStorage.getItem("id");
            document.getElementById("register").href = "#";
            document.getElementById("login").textContent = "Log off";
            document.getElementById("login").onclick = logOff;
            document.getElementById("login").href = "#";
        }
        //else {
        //    $("#logout").hide();
        //}
    });
});

function logOff() {
    sessionStorage.removeItem("id");
    window.location.replace("HomePage.html");
}