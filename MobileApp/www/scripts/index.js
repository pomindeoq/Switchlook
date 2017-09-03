// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in cordova-simulate or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
var Pages = new Pages();
var NavigationTop = new NavigationTop();
var User = new User();


(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {

        $(function () {
            $(window).bind('hashchange', function () {
            }).trigger('hashchange');
        });

        if (window.location.hash === '') {
            window.location.hash = "/main"; // home page, show the default view (#main
        } else {
            console.log('trigger hashchange');
            $(window).trigger("hashchange"); // user refreshed the browser, fire the appropriate function
        }

        /* PAGES TO LOAD */
        // MAIN
        (function () {
            var handler = function() {
                $('#testing').html('working....');
            };

            var navigationTopData = new NavigationTopData(false, '', false, '', true, 'Switchlook');

            var page = new Page("Main", "/main", "pages/main.html", handler, true, true, navigationTopData);
            Pages.AddPage(page);
        }());

        // LOGIN
        (function () {
            var handler = function() {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<span class="icon icon-arrow-left2"></span>', false, '', true, 'Login');

            var page = new Page("Login", "/login", "pages/login/index.html", handler, false, true, navigationTopData);
            Pages.AddPage(page);
        }());

        // PROFILE
        (function () {
            var handler = function () {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/settings"><span class="icon icon-menu"></span></a>', false, '', true, 'Profile');

            var page = new Page("Profile", "/profile", "pages/profile/index.html", handler, true, true, navigationTopData);
            Pages.AddPage(page);
        }());

        // SETTINGS
        (function () {
            var handler = function () {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/profile"><span class="icon icon-arrow-left2"></span></a>', false, '', true, 'Settings');

            var page = new Page("Settings", "/settings", "pages/settings/index.html", handler, true, true, navigationTopData);
            Pages.AddPage(page);
        }());

        // SIGN UP
        (function () {
            var handler = function () {
                //
            };

            var page = new Page("Signup", "/signup", "pages/signup/index.html", handler, false, false);
            Pages.AddPage(page);
        }());

        console.log(Pages);



        (function () {
            $(document).on("click", "#signUpBtn", function (e) {
                e.preventDefault();

                var datastring = $('form#signUpForm').serialize();

                alert(datastring);

                var name = $('#name').val();

                alert(name);

                //jQuery.ajax({
                //    type: "POST",
                //    url: 'localhost:54443/api/account/register',
                //    data: dataString,
                //    cache: false,
                //    async: true,
                //    error: function (XMLHttpRequest, textStatus, errorThrown) {
                //        alert(textStatus + ": " + errorThrown);
                //    }
                //}).done(function (returned) {
                //    obj = JSON.parse(returned);
                    
                //});

                console.log("signUpBtn");
            });
        }());

        // add router handler to hashchange event
        $(window).on('hashchange', router.start);

        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        //var parentElement = document.getElementById('deviceready');
        //var listeningElement = parentElement.querySelector('.listening');
        //var receivedElement = parentElement.querySelector('.received');
        //listeningElement.setAttribute('style', 'display:none;');
        //receivedElement.setAttribute('style', 'display:block;');
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };

    

})();

