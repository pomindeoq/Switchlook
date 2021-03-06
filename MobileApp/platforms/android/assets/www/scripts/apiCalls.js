﻿function WebApi() {

}

WebApi.prototype.OnError = function (XMLHttpRequest, textStatus, errorThrown) {
    switch (XMLHttpRequest.status) {
    case 401:
        this.OnUnauthorizedAccess();
        break;
    default:
        console.log(textStatus + ": " + errorThrown + " Code: " + XMLHttpRequest.status);
    }
}

WebApi.prototype.Call = function(type, data, url, async = true, successHanlder) {
    switch (type) {
    case "GET":
        jQuery.ajax({
            type: "GET",
            url: 'http://localhost:54443/api/' + url,
            cache: false,
            async: async,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                API.OnError(XMLHttpRequest, textStatus, errorThrown);
            }
        }).done(function (returned) {
            successHanlder(returned);
        });
        break;
    case "POST":
        jQuery.ajax({
            type: "POST",
            data: data,
            contentType: "application/json",
            url: 'http://localhost:54443/api/' + url,
            cache: false,
            async: async,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                API.OnError(XMLHttpRequest, textStatus, errorThrown);
            }
        }).done(function (returned) {
            successHanlder(returned);
        });
        break;
    }
}



WebApi.prototype.OnUnauthorizedAccess = function() {
    console.log("Unauthorized");
}

WebApi.prototype.FacebookRegister = function(data, handler) {
    this.Call(
        "POST",
        data,
        "account/facebookRegister",
        true,
        function (returned) {
            handler(returned);
        }
    );
}

WebApi.prototype.SignInFacebook = function (data, handler) {
    this.Call(
        "POST",
        data,
        "account/facebookSignIn",
        true,
        function (returned) {
            handler(returned);
        }
    );
}

WebApi.prototype.GoogleRegister = function (data, handler) {
    this.Call(
        "POST",
        data,
        "account/googleRegister",
        true,
        function (returned) {
            handler(returned);
        }
    );
}

WebApi.prototype.SignInGoogle = function(data, handler) {
    this.Call(
        "POST",
        data,
        "account/googleSignIn",
        true,
        function(returned) {
            handler(returned);
        }
    );
}

WebApi.prototype.SignIn = function (data, handler) {
    this.Call(
        "POST",
        data,
        "account/login",
        true,
        function (returned) {
            handler(returned);
        }
    );
}

WebApi.prototype.RegisterAccount = function (data, handler) {
    this.Call(
        "POST",
        data,
        "account/register",
        true,
        function (returned) {
            handler(returned);
        }
    );
}

WebApi.prototype.SignOut = function() {
    this.Call(
        "GET",
        null,
        "account/signOut",
        true,
        function () {
            User.IsAuthinticated = false;
            App.Pages.GoTo("/signin");
        }
    );
}

WebApi.prototype.IsUserAuthenticated = function() {
    var result = false;
    this.Call(
        "GET",
        null,
        "account/isAuthenticated",
        false,
        function (returned) {
            console.log(returned);
            result = returned;
        }
    );
    return result;
}
