// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in cordova-simulate or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
var App = new Application();
var API = new WebApi();
var User;


(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {

        // Init User object
        User = new User();

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


        /* SEPERATE THIS LATER */
        /* PAGES TO LOAD */
        // MAIN
        (function () {
            var handler = function() {
                $('#testing').html('working....');
            };

            var navigationTopData = new NavigationTopData(false, '', false, '', true, 'Switchlook');

            var page = new Page("Main", "/main", "pages/main.html", handler, true, true, navigationTopData);
            App.Pages.AddPage(page);
        }());

        // LOGIN
        (function () {
            var handler = function() {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/signin"><span class="icon icon-arrow-left2"></span></a>', false, '', true, 'Login');

            var page = new Page("Login", "/login", "pages/login/index.html", handler, false, true, navigationTopData, false);
            App.Pages.AddPage(page);
        }());

        // PROFILE
        (function () {
            var handler = function () {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/settings"><span class="icon icon-menu"></span></a>', false, '', true, 'Profile');

            var page = new Page("Profile", "/profile", "pages/profile/index.html", handler, true, true, navigationTopData);
            App.Pages.AddPage(page);
        }());

        // SETTINGS
        (function () {
            var handler = function () {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/profile"><span class="icon icon-arrow-left2"></span></a>', false, '', true, 'Settings');

            var page = new Page("Settings", "/settings", "pages/settings/index.html", handler, true, true, navigationTopData);
            App.Pages.AddPage(page);
        }());

        // SIGN UP
        (function () {
            var handler = function () {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/login"><span class="icon icon-arrow-left2"></span></a>', false, '', true, 'Signup');

            var page = new Page("Signup", "/signup", "pages/signup/index.html", handler, false, true, navigationTopData, false);
            App.Pages.AddPage(page);
        }());

        // EDIT PROFILE
        (function () {
            var handler = function () {
                //
            };

            var navigationTopData = new NavigationTopData(true, '<a href="#/settings"><span class="icon icon-arrow-left2"></span></a>', false, '', true, 'Edit Profile');

            var page = new Page("Edit Profile", "/edit_profile", "pages/edit_profile/index.html", handler, true, true, navigationTopData, true);
            App.Pages.AddPage(page);
        }());

        // SIGN IN
        (function () {
            var handler = function () {
                
                if (googleAuth2 === null) {
                    gapi.load('auth2',
                        function() {
                            // Retrieve the singleton for the GoogleAuth library and set up the client.  
                            googleAuth2 = gapi.auth2.init({
                                client_id: '697330306989-6ur1rkdq2brslp5nvglurri3qj1025ut.apps.googleusercontent.com',
                                cookiepolicy: 'single_host_origin', /** Default value **/
                                scope:
                                    'profile' // Request scopes in addition to 'profile' and 'email'      scope: 'additional_scope'
                            }).then(function(auth2) {
                                startApp();
                            });

                        });
                } else {
                    startApp();
                }
                              
            };

            var page = new Page("Signin", "/signin", "pages/signin/index.html", handler, false, false, null, false);
            App.Pages.AddPage(page);
        }());

        // EXTERNAL REGISTER CONFIRMATION
        (function () {
            var handler = function () {
                if (!User.ExternalRegisterConfirmation)
                    App.Pages.GoTo("/main");

                if (googleAuth2 === null) {
                    gapi.load('auth2',
                        function () {
                            // Retrieve the singleton for the GoogleAuth library and set up the client.  
                            googleAuth2 = gapi.auth2.init({
                                client_id: '697330306989-6ur1rkdq2brslp5nvglurri3qj1025ut.apps.googleusercontent.com',
                                cookiepolicy: 'single_host_origin', /** Default value **/
                                scope:
                                    'profile' // Request scopes in addition to 'profile' and 'email'      scope: 'additional_scope'
                            });

                        });
                }
            };

            var navigationTopData = new NavigationTopData(false, '', false, '', true, 'Signup');

            var page = new Page("ExternalRegister", "/externalRegister", "pages/signin/registrationConfirmation.html", handler, false, true, navigationTopData, false);
            App.Pages.AddPage(page);
        }());

        console.log(App.Pages);




        
        (function () {

            // Default register call
            $(document).on("click", "#signUpBtn", function (e) {
                e.preventDefault();

                var datastring = $('form#signUpForm').serializeFormJSON();
                

                var termsOfUse = false;

                if ($('#TermsOfUse').is(":checked")) {
                    termsOfUse = true;
                }
                datastring.TermsOfUse = termsOfUse;
                var receiveEmails = false;

                if ($('#ReceiveEmails').is(":checked")) {
                    receiveEmails = true;
                }
                datastring.ReceiveEmails = receiveEmails;
                var jsonString = JSON.stringify(datastring);

                console.log(jsonString);

                API.RegisterAccount(
                    jsonString,
                    function (returned) {
                        $("#errors").empty();
                        if (returned.isModelValid) {
                            if (returned.succeeded) {
                                User.IsAuthinticated = true;
                                App.Pages.GoTo("/main");
                            } else {
                                returned.errors.forEach(function (item) {
                                    $("#errors").append("<p>" + item + "</p>");
                                });
                            }
                        } else {
                            returned.errors.forEach(function (item) {
                                $("#errors").append("<p>" + item + "</p>");
                            });
                        }
                    }
                );

                console.log("signUpBtn");
            });

            // Default login call
            $(document).on("click", "#loginBtn", function (e) {
                e.preventDefault();

                var datastring = $('form#loginForm').serializeFormJSON();

                var jsonStr = JSON.stringify(datastring);

                console.log(jsonStr);
                
                API.SignIn(
                    jsonStr,
                    function(returned) {
                        console.log(returned);
                        if (returned.isModelValid) {
                            if (returned.result.succeeded) {
                                User.IsAuthinticated = true;
                                App.Pages.GoTo("/main");
                            }
                        } else {
                            $("#errors").empty();
                            returned.errors.forEach(function (item) {
                                $("#errors").append("<p>" + item + "</p>");
                            });
                        }
                    }
                );


                console.log("loginBtn");
            });

            // Sign out call
            $(document).on("click", "#signOut", function (e) {
                e.preventDefault();

                API.SignOut();
               
                console.log("signOutLink");
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

