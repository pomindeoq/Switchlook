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

function API_signIn(data) {
    callAPI(
        "POST",
        data,
        "account/login",
        true,
        function (returned) {
            var obj = JSON.parse(returned);
            console.log(obj);

            //Pages.GoTo("/main");
        }
    );
}

function API_registerAccount(data) {
    callAPI(
        "POST",
        data,
        "account/register",
        true,
        function(returned) {


            // redirect to somewhere when user registers
        }
    );
}


function API_signOut() {
    callAPI(
        "GET",
        null,
        "account/SignOut",
        true,
        function(returned) {
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
