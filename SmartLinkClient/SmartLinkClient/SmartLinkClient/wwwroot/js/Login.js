class LoginManager {
    constructor(loginBtn, logoutBtn, loginModal, loginBtnModal) {
        this.loginBtn = loginBtn;
        this.logoutBtn = logoutBtn;
        this.loginModal = loginModal;
        this.loginBtnModal = loginBtnModal;
    }
    init() {
        this.loginBtn.addEventListener("click", () => this.showLoginModal());
        this.loginBtnModal.addEventListener("click",() => this.login());
        this.logoutBtn.addEventListener("click",() => this.logout());
    }

    showLoginModal() {
        this.loginModal.show();
    }

    hideLoginModal() {
        this.loginModal.hide();
    }

    async login() {
        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;

        if (username == "" || password == "") {
            showToast("ERROR", "Username or password cannot be empty.");
            return;
        }
        try {
            const response = await fetch("https://smartlinkapi.imaginewebsite.com.tr/api/users/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ username: username, password: password })
            });
            if (!response.ok) {
                showToast("ERROR", "Username or password is incorrect.");
                throw new Error(`Response Status: ${response.status}`);
            }
            const json = await response.json();
            localStorage.setItem("username", username);
            localStorage.setItem("jwt", json.token);
            showToast("SUCCESS", "Login successful.");
            this.hideLoginModal();
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        }
        catch (error) {
            console.log(error.message);
        }
    }

    logout(){
        localStorage.clear();
        window.location.reload();
    }
}