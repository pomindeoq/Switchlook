function Ui() {
    this.Pages = new Pages();
    this.NavigationTop = new NavigationTop();
    this.LoadingOverlay = new LoadingOverlay();
}

function LoadingOverlay() {
    this.Show = function() {
        $('body').css('overflow', 'hidden');
        $('#LoadingOverlay').css('display', 'block');
    }
    this.Hide = function() {
        $('body').css('overflow', '');
        $('#LoadingOverlay').fadeOut(250);
    }
}