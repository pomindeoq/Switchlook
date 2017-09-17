function Application() {
    this.Pages = new Pages();
    this.NavigationTop = new NavigationTop();
    this.NavigationBottom = new NavigationBottom();
    this.LoadingOverlay = new LoadingOverlay();
}

function LoadingOverlay() {
    this.Status = false;
    this.Show = function () {
        if (this.Status === false) {
            $('body').css('overflow', 'hidden');
            $('#LoadingOverlay').css('display', 'block');
            this.Status = true;
        }    
    }
    this.Hide = function () {
        if (this.Status === true) {
            $('body').css('overflow', '');
            $('#LoadingOverlay').fadeOut(250);
            this.Status = false;
        }
    }
}