function User() {
    this.IsAuthinticated = API.IsUserAuthenticated();
    this.ExternalRegisterConfirmation = false;
    this.ExternalRegisterType = null;
}
