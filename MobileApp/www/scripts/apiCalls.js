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
        alert(textStatus + ": " + errorThrown + " Code: " + XMLHttpRequest.status);
    }
}

function OnUnauthorizedApiAccess() {
    alert("Unauthorized");
}

function isUserAuthenticated() {
    callAPI(
        "GET",
        null,
        "account/isAuthenticated",
        false,
        function(returned) {
            alert(returned);
        }
    );
}
