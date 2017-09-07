function callAPI(type, data, url, async = true, successHanlder) {
    switch(type) {
        case "GET":
            jQuery.ajax({
                type: "GET",
                url: 'http://localhost:54443/api/' + url,
                cache: false,
                async: async,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    OnApiError(XMLHttpRequest, textStatus, errorThrown);
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
                    OnApiError(XMLHttpRequest, textStatus, errorThrown);
                }
            }).done(function (returned) {
                successHanlder(returned);
            });
            break;
    }
}

function OnApiError(XMLHttpRequest, textStatus, errorThrown) {
    switch (XMLHttpRequest.status) {
    case 401:
        OnUnauthorizedApiAccess();
        break;
    default:
        console.log(textStatus + ": " + errorThrown + " Code: " + XMLHttpRequest.status);
    }
}

function OnUnauthorizedApiAccess() {
    console.log("Unauthorized");
}

function API_signInFacebook(data, handler) {
    callAPI(
        "POST",
        data,
        "account/facebookSignIn",
        true,
        function (returned) {
            handler(returned);
        }
    );
}

function API_signIn(data, handler) {
    callAPI(
        "POST",
        data,
        "account/login",
        true,
        function (returned) {
            handler(returned);
            //Pages.GoTo("/main");
        }
    );
}

function API_registerAccount(data, handler) {
    callAPI(
        "POST",
        data,
        "account/register",
        true,
        function(returned) {
            handler(returned);

            // redirect to somewhere when user registers
        }
    );
}


function API_signOut() {
    callAPI(
        "GET",
        null,
        "account/signOut",
        true,
        function (returned) {
            User.IsAuthinticated = false;
            Pages.GoTo("/login");
        }
    );
}

function API_isUserAuthenticated() {
    var result = false;
    callAPI(
        "GET",
        null,
        "account/isAuthenticated",
        false,
        function(returned) {
            console.log(returned);
            result = returned;
        }
    );
    return result;
}
