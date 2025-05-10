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
        const email = document.getElementById("email").value;
        const password = document.getElementById("password").value;

        if (email == "" || password == "") {
            showToast("ERROR", "Email or password cannot be empty.");
            return;
        }
        try {
            const response = await fetch("https://smartlinkapi.imaginewebsite.com.tr/api/users/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ email: email, password: password })
            });
            const json = await response.json();
            if (!response.ok) {
                showToast("ERROR", json.message);
            }
            localStorage.setItem("email", email);
            localStorage.setItem("jwt", json.token);
            showToast("SUCCESS", json.message);
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