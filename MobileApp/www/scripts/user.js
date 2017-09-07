function User() {
    this.IsAuthinticated = API_isUserAuthenticated();
    this.ExternalRegisterConfirmation = false;
    this.ExternalLoginData = null;
}

function ExternalLoginData(type, data) {
    this.Type = type;
    this.Data = data;
}
