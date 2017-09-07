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

function statusChangeCallback(response) {
    console.log(response);
    if (response.status === 'connected') {
        FB.api(
            '/me',
            'GET',
            { "fields": "id,name,first_name,last_name,email" },
            function (response2) {
                console.log(response2);
                var data = {
                    Id: response2.id,
                    Name: response2.name,
                    FirstName: response2.first_name,
                    LastName: response2.last_name,
                    Email: response2.email
                };


                var externalLoginData = new ExternalLoginData('Facebook', data);
                User.ExternalLoginData = externalLoginData;

                var jsonString = JSON.stringify(data);
                console.log(jsonString);
                API_signInFacebook(
                    jsonString,
                    function(returned) {
                        if (returned.isModelValid) {
                            if (returned.result.succeeded) {
                                User.IsAuthinticated = true;
                                Pages.GoTo("/main");
                            } else {
                                if (!returned.isRegistered) {
                                    User.ExternalRegisterConfirmation = true;

                                    Pages.GoTo("/externalRegister");
                                }
                            }
                        } else {
                            $("#errors").empty();
                            returned.errors.forEach(function (item) {
                                $("#errors").append("<p>" + item + "</p>");
                            });
                        }
                    }
                );
            }
        );
        console.log('Logged in and authenticated');
    } else {
        console.log('Not autheticated');
    }
};

function FBcheckLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
};