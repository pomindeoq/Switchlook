// initialize and setup facebook js sdk
var fbAppId = 496342054036347;

window.fbAsyncInit = function () {
    FB.init({
        appId: '496342054036347',
        autoLogAppEvents: true,
        xfbml: true,
        version: 'v2.10'
    });
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


function facebook_getUserAccessToken(callback) {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            callback(response.authResponse.accessToken);
        } else {
            console.log('Not autheticated');
        }
    });
}

function statusChangeCallback(response) {
    console.log(response);
    if (response.status === 'connected') {
        console.log(response.authResponse.accessToken);

        var data = {
            AccessToken: response.authResponse.accessToken
        }


        var jsonString = JSON.stringify(data);
        console.log(jsonString);

        App.LoadingOverlay.Show();
        API.SignInFacebook(
            jsonString,
            function(returned) {
                if (returned.isModelValid) {
                    if (returned.result.succeeded) {
                        User.IsAuthinticated = true;
                        facebookLogout();
                        App.Pages.GoTo("/main");
                    } else {
                        if (!returned.isRegistered) {
                            User.ExternalRegisterConfirmation = true;
                            User.ExternalRegisterType = "Facebook";
                            User.ExternalLoginToken = data.AccessToken;
                            App.Pages.GoTo("/externalRegister");
                        }
                    }
                } else {
                    $("#errors").empty();
                    returned.errors.forEach(function (item) {
                        $("#errors").append("<p>" + item + "</p>");
                    });
                }
                App.LoadingOverlay.Hide();
            }
        );
    } else {
        console.log('Not autheticated');
    }
};
function FBcheckLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
};

function deleteCookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

function facebookLogout() {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            FB.logout(function (response) {
                console.log('user logged out');
            });
        }
    });
    deleteCookie("fblo_" + fbAppId);
}

$(document).on("click", "#FB-button", function (e) {
    e.preventDefault();
    facebookLogin();
    console.log("facebook login");
});

function facebookLogin() {
    FB.login(function (response) {
        if (response.status === 'connected') {
            statusChangeCallback(response);
        }
    }, { scope: 'public_profile,email' }); 
}


// Google button //

var googleAuth2 = null;

function startApp() {
    attachSignin(document.getElementById('customBtn'));
}

function google_getUserAccessToken() {

    var auth2 = gapi.auth2.getAuthInstance();
    var googleUser = auth2.currentUser.get();

    var id_token = googleUser.getAuthResponse().id_token;

    return id_token;
}

function onSignIn(googleUser) {
    var id_token = googleUser.getAuthResponse().id_token;
    console.log(id_token);

    var data = {
        AccessToken: id_token
    }

    var jsonString = JSON.stringify(data);
    console.log(jsonString);

    App.LoadingOverlay.Show();
    API.SignInGoogle(
        jsonString,
        function (returned) {
            if (returned.isModelValid) {
                if (returned.result.succeeded) {
                    User.IsAuthinticated = true;
                    gapi.auth2.getAuthInstance().signOut();
                    App.Pages.GoTo("/main");
                } else {
                    if (!returned.isRegistered) {
                        User.ExternalRegisterConfirmation = true;
                        User.ExternalRegisterType = "Google";
                        App.Pages.GoTo("/externalRegister");
                    }
                }
            } else {
                $("#errors").empty();
                returned.errors.forEach(function (item) {
                    $("#errors").append("<p>" + item + "</p>");
                });
            }
            App.LoadingOverlay.Hide();
        }
    );
}

function attachSignin(element) {
    console.log(element.id);

    var auth2 = gapi.auth2.getAuthInstance();
    auth2.attachClickHandler(element, {},
        function (googleUser) {
            onSignIn(googleUser);
        }, function (error) {
            alert(JSON.stringify(error, undefined, 2));
        });
}
  
    
// External register confirmation
$(document).on("click", "#regConfirmationBtn", function (e) {
    e.preventDefault();

    if (User.ExternalRegisterConfirmation) {
        App.LoadingOverlay.Show();
        if (User.ExternalRegisterType === "Facebook") {
            facebook_getUserAccessToken(function (token) {
                var username = $('#username').val();

                var data = {
                    AccessToken: token,
                    Username: username
                }
                var jsonStr = JSON.stringify(data);

                console.log(jsonStr);

                API.FacebookRegister(
                    jsonStr,
                    function (returned) {
                        $("#errors").empty();
                        console.log(returned);
                        if (returned.isModelValid) {
                            if (returned.createResult.succeeded) {
                                User.IsAuthinticated = true;
                                facebookLogout();
                                App.Pages.GoTo("/main");
                            } else {
                                App.LoadingOverlay.Hide();
                                returned.errors.forEach(function (item) {
                                    $("#errors").append("<p>" + item + "</p>");
                                });
                            }
                        } else {
                            App.LoadingOverlay.Hide();
                            returned.errors.forEach(function (item) {
                                $("#errors").append("<p>" + item + "</p>");
                            });
                        }
                    }
                );

            });

        }
        else if (User.ExternalRegisterType === "Google") {

            var token = google_getUserAccessToken();
            var username = $('#username').val();
            var data = {
                AccessToken: token,
                Username: username
            }
            var jsonStr = JSON.stringify(data);
            console.log(jsonStr);
            API.GoogleRegister(
                jsonStr,
                function (returned) {
                    $("#errors").empty();
                    console.log(returned);
                    if (returned.isModelValid) {
                        if (returned.createResult.succeeded) {
                            User.IsAuthinticated = true;
                            gapi.auth2.getAuthInstance().signOut();
                            App.Pages.GoTo("/main");
                        } else {
                            App.LoadingOverlay.Hide();
                            returned.errors.forEach(function (item) {
                                $("#errors").append("<p>" + item + "</p>");
                            });
                        }
                    } else {
                        App.LoadingOverlay.Hide();
                        returned.errors.forEach(function (item) {
                            $("#errors").append("<p>" + item + "</p>");
                        });
                    }
                }
            );

        }



    }

    console.log("ExternalRegisterConfirmationClick");
});