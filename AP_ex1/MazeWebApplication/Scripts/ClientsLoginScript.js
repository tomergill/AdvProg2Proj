/**
 * CHecks the input
 */
function InputCheck() {
    var pass = $("#password").val();
    var user = $("#userName").val();
    if (pass.length >= 6 && user.length >= 2) {
        return true;
    }
    return false;
}

//validates input
$(document).ready(function () {
    if (sessionStorage.getItem('id') == null) {
        $.validator.setDefaults({
            highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            }
        });

        $.validator.addMethod('strongPassword', function (value, element) {
            return this.optional(element) || (value.lenght >= 6 && /\d/.test(value) && /[a-z]/i.test(value));
        }, 'your password must be at least 6 characters long and contain at least one digit and one latter.')

        // validate signup form on keyup and submit
        $("#UserLogin").validate({
            rules: {
                userName: {
                    required: true,
                    minlength: 2
                },
                password: {
                    required: true,
                    minlength: 6
                },
            },
            messages: {
                userName: {
                    required: "Enter a username",
                    minlength: jQuery.validator.format("Enter at least {0} characters"),
                },
                password: {
                    required: "Provide a password",
                    minlength: jQuery.validator.format("Enter at least {0} characters"),

                },
            },
            // specifying a submitHandler prevents the default submit, good for the demo
            submitHandler: function () {
                if (InputCheck()) {
                    alert("submitted!");
                }
            },
        });
    }
    else {
        window.location.replace("HomePage.html");
    }
});

/**
 * After login saves in session the user name and redirects to home page
 * @param {any} event
 */
function LoginCompleted(event) {
    $("#loader").show();
    event.preventDefault();
    if (InputCheck()) {
        var data = {
            UserName: $("#userName").val(),
            Password: $("#password").val()
        };

        var url = "../api/User/" + data.UserName + "/" + data.Password;
        $.get(url, data.UserName, data.Password)
            .done(function (user) {
                sessionStorage.setItem("id", $("#userName").val());
                window.location.replace("HomePage.html");
                $("#loader").hide();
            })
            .fail(function (jqXHR, status, error) {
                console.error("post failed, status: " + status + ", error: " + error);
                alert("failed");
                $("#loader").hide();
            });
    }
}