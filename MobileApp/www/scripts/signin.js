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