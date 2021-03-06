﻿/**
 * Checks the input
 */
function InputCheck() {
    var pass = $("#password").val();
    var validPass = $("#validateUserName").val();
    var user = $("#userName").val();
    var mail = $("#emailAdress").val();
    if (pass == validPass && pass.length >= 6 && user.length >= 2 && mail.includes('@')) {
        return true;
    }
    return false;
}

$(document).ready(function () {
    $("#loader").hide();
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
    $("#UserRegistration").validate({
        rules: {
            userName: {
                required: true,
                minlength: 2
            },
            password: {
                required: true,
                minlength: 6
            },
            validateUserName: {
                required: true,
                minlength: 6,
                equalTo: "#password"
            },
            emailAdress: {
                required: true,
                email: true
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
            validateUserName: {
                required: "Repeat your password",
                minlength: jQuery.validator.format("Enter at least {0} characters"),
                equalTo: "Enter the same password as above"
            },
            emailAdress: {
                required: "Please enter a valid email address",
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

/**
 * After register saves the user name is the session and redirects to the home page
 * @param {any} event
 */
function RegisterCompleted(event) {
    event.preventDefault();
    if (InputCheck()) {
        var data = {
            UserName: $("#userName").val(),
            Password: $("#password").val(),
            Wins: 0,
            Losses: 0,
            EmailAdress: $("#emailAdress").val()
        };
        $("#loader").show();
        $.post("../api/User", data)
            .done(function (user) {
                $("#loader").hide();
                sessionStorage.setItem("id", $("#userName").val());
                window.location.replace("HomePage.html");
            })
            .fail(function (jqXHR, status, error) {
                $("#loader").hide();
                console.error("post failed, status: " + status + ", error: " + error);
                alert("failed");
            });
    }
}