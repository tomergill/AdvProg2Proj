$(document).ready(function loadBar() {
    $("#navbar").load("NavigationBar.html", function () {
        if (sessionStorage.getItem("UserName")) {
            document.getElementById("register").textContent = "Hello " + sessionStorage.getItem("UserName");
            document.getElementById("register").href = "#";
            document.getElementById("login").textContent = "Log off";
            document.getElementById("login").onclick = logOff;
            document.getElementById("login").href = "Main.html";
        }
    });
});


function logOff() {
    sessionStorage.removeItem("UserName");
}
