// initialize and setup facebook js sdk
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

        UI.LoadingOverlay.Show();
        API.SignInFacebook(
            jsonString,
            function(returned) {
                if (returned.isModelValid) {
                    if (returned.result.succeeded) {
                        User.IsAuthinticated = true;
                        UI.Pages.GoTo("/main");
                    } else {
                        if (!returned.isRegistered) {
                            User.ExternalRegisterConfirmation = true;
                            User.ExternalRegisterType = "Facebook";
                            UI.Pages.GoTo("/externalRegister");
                        }
                    }
                } else {
                    $("#errors").empty();
                    returned.errors.forEach(function (item) {
                        $("#errors").append("<p>" + item + "</p>");
                    });
                }
                UI.LoadingOverlay.Hide();
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
 // Google button //

gapi.load('auth2', function () {
    // Retrieve the singleton for the GoogleAuth library and set up the client.  
    gapi.auth2.init({
        client_id: '697330306989-6ur1rkdq2brslp5nvglurri3qj1025ut.apps.googleusercontent.com',
        cookiepolicy: 'single_host_origin', /** Default value **/
        scope: 'profile'  // Request scopes in addition to 'profile' and 'email'      scope: 'additional_scope'
    });
    
});

function startApp(){
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

    UI.LoadingOverlay.Show();
    API.SignInGoogle(
        jsonString,
        function (returned) {
            if (returned.isModelValid) {
                if (returned.result.succeeded) {
                    User.IsAuthinticated = true;
                    gapi.auth2.getAuthInstance().signOut();
                    UI.Pages.GoTo("/main");
                } else {
                    if (!returned.isRegistered) {
                        User.ExternalRegisterConfirmation = true;
                        User.ExternalRegisterType = "Google";
                        UI.Pages.GoTo("/externalRegister");
                    }
                }
            } else {
                $("#errors").empty();
                returned.errors.forEach(function (item) {
                    $("#errors").append("<p>" + item + "</p>");
                });
            }
            UI.LoadingOverlay.Hide();
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
  
    
