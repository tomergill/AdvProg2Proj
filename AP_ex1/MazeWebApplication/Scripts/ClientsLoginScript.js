
function InputCheck() {
    var pass = $("#password").val();
    var user = $("#userName").val();
    if (pass.length >= 6 && user.length >= 2) {
        return true;
    }
    return false;
}


$(document).ready(function () {
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
});


function LoginCompleted(event) {
    event.preventDefault();
    if (InputCheck()) {
        var data = {
            UserName: $("#userName").val(),
            Password: $("#password").val(),
        };

        var url = "../api/User" + "/" + data.userName;
        $.get(url, data)
            .done(function (user) {
                alert("success");
            })
            .fail(function (jqXHR, status, error) {
                console.error("post failed, status: " + status + ", error: " + error);
                alert("post failed, status: " + status + ", error: " + error);
            });
    }
}