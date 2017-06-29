
window.onload = function getAllUsers()
{
    var url = "../api/User";
    $("#loader").show();
    $.get(url)
        .done(function (usersList) {
            $("#loader").hide();
            var tr;
            for (var i = 0; i < usersList.length; i++) {
                tr = $('<tr/>');
                tr.append("<th>" + (usersList[i].Wins - usersList[i].Losses)  + "</th>");
                tr.append("<th>" + usersList[i].UserName + "</th>");
                tr.append("<th>" + usersList[i].Wins + "</th>");
                tr.append("<th>" + usersList[i].Losses + "</th>");

                var dt = new Date(usersList[i].FirstSignedIn);
                tr.append("<th>" + (dt.getMonth() + 1) + '/' +
                    dt.getDate() + '/' + dt.getFullYear() + "</th>");

                $("#usersTable").append(tr);
            }
        })
        .fail(function (jqXHR, status, error) {
            console.error("post failed, status: " + status + ", error: " + error);
            alert("post failed, status: " + status + ", error: " + error);
            $("#loader").hide();
        });
}