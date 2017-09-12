/* EXAMPLE: How to add a page!

(function () {
    var handler = function() {
        // // inside here you can put methods etc.. This function is executed after page is loaded from the HTML file.
    };

    var navigationTopData = new NavigationTopData(true, '<span class="icon icon-arrow-left2"></span>', false, '', true, 'Login'); // setting up top navigation

    var page = new Page("Login", "/login", "pages/login/index.html", handler, true, navigationTopData);
    UI.Pages.AddPage(page);
}());

(function () {
    var handler = function () {
        $('#testing').html('working....') // inside here you can put methods etc.. This function is executed after page is loaded from the HTML file.
    }

    var page = new Page("Main", "Switchlook", "/main", "pages/main.html", handler);
    UI.Pages.AddPage(page);
}());

*/

function Pages() {
    this.CurrentPage = ""; // route
    this.PreviousPage = ""; // route
    this.Pages = [];
    this.AddPage = function(page) {
        this.Pages.push(page);
    };
}

Pages.prototype.GoBack = function() {
    if (this.PreviousPage !== "") {
        router.load(this.PreviousPage);
    }
}

Pages.prototype.GoTo = function (route) {
    router.load(route);
}

Pages.prototype.IsAuthToAccess = function (restricted, auth) {
    if (restricted === true && auth === true) {
        return true;
    }else if (restricted === false) {
        return true;
    } else {
        return false;
    }
}

Pages.prototype.SwitchPage = function(route) {
    this.PreviousPage = this.CurrentPage;
    this.CurrentPage = route;
    console.log("CurrentPage: " + this.CurrentPage + " PreviousPage: " + this.PreviousPage);
}

function NavigationTopData(leftShow = true, leftContent, rightShow = true, rightContent, titleShow = true, titleContent) {
    this.Left = { Show: leftShow, Content: leftContent };
    this.Right = { Show: rightShow, Content: rightContent };
    this.Title = { Show: titleShow, Content: titleContent };
}

function NavigationTop() {
    this.Status = false;
    this.Data = new NavigationTopData(false, '', false, '', false, '');
}

NavigationTop.prototype.Show = function(data) {
    this.Data = data;
    if (!this.Status) {
        $('#header').show();
        $(document.body).css('padding-top', '12%');
        this.Fill();
        this.Status = true;
    } else {
        this.Fill();
        this.Status = true;
    }
};

NavigationTop.prototype.Hide = function() {
    this.ResetData();
    $('#header').hide();
    $(document.body).css('padding-top', '0%');
    this.Status = false;
    console.log('NavigationTop Hidden');
};

NavigationTop.prototype.Fill = function() {
    if (this.Data.Left.Show) {
        $('#navigation-top-left').html(this.Data.Left.Content);
        console.log('NavigationTop Left Filled');
    } else $('#navigation-top-left').empty();
    if (this.Data.Title.Show) {
        $('#navigation-top-title').html(this.Data.Title.Content);
        console.log('NavigationTop Title Filled');
    } else $('#navigation-top-title').empty();
    if (this.Data.Right.Show) {
        $('#navigation-top-right').html(this.Data.Right.Content);
        console.log('NavigationTop Right Filled');
    } else $('#navigation-top-right').empty();
    console.log('NavigationTop Filled');
    console.log(this.Data);
};

NavigationTop.prototype.ResetData = function() {
    this.Data = new NavigationTopData(false, '', false, '', false, '');
};

function Page(name, route, url, handler, reqAuthentication = true, showNavigationTop = true, navigationTopData = new NavigationTopData(), showNavigationBar = true) {
    this.Url = url;
    this.Route = route;
    this.Name = name;
    this.ShowNavigationTop = showNavigationTop;
    this.NavigationTopData = navigationTopData;
    this.NavigationBottom = { Show: showNavigationBar };
    this.Handler = handler;
    this.ReqAuthentication = reqAuthentication;

    var tempReqAuthentication = this.ReqAuthentication;
    var tempRoute = this.Route;
    var tempNavigationTopData = this.NavigationTopData;
    var tempShowNavigationTop = this.ShowNavigationTop;
    var tempHanlder = this.Handler, tempUrl = this.Url;
    router.addRoute(this.Route, function () {

        if (UI.Pages.IsAuthToAccess(tempReqAuthentication, User.IsAuthinticated)) {
            UI.Pages.SwitchPage(tempRoute);
            UI.LoadingOverlay.Show();
            if (tempShowNavigationTop) {
                NavigationTop.Show(tempNavigationTopData);
            } else {
                NavigationTop.Hide();
            }
            $('#app').load(tempUrl,
                function () {
                    tempHanlder();
                    UI.LoadingOverlay.Hide();
                });
        } else {
            UI.Pages.GoTo("/login");
        }
    });
}