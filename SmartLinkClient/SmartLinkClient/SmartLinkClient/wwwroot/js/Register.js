class RegisterManager {
    constructor(registerBtn, registerModal, registerModalBtn) {
        this.registerBtn = registerBtn;
        this.registerModal = registerModal;
        this.registerModalBtn = registerModalBtn;
    }
    init() {
        this.registerBtn.addEventListener("click", () => this.showRegisterModal());
        this.registerModalBtn.addEventListener("click", () => this.register());
    }

    showRegisterModal() {
        this.registerModal.show();
    }

    hideRegisterModal() {
        this.registerModal.hide();
    }

    async register() 
    {
        const email = document.getElementById("register-email").value;
        const username = document.getElementById("register-username").value;
        const password = document.getElementById("register-password").value;
        const repeatPassword = document.getElementById("password-repeat").value;
        if (!username || !password || !repeatPassword || !email) {
            showToast("ERROR", "All fields are required.");
            return;
        }
        if (password != repeatPassword) {
            showToast("ERROR", "Passwords do not match.");
            return;
        }
        const response = await fetch("https://smartlinkapi.imaginewebsite.com.tr/api/users/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ email: email,username: username, password: password })
        });
        if (!response.ok) {
            const json = await response.json();
            showToast("ERROR", json.message);
            return;
        }
        showToast("SUCCESS", "Register was success, now you can login.");

        this.hideRegisterModal();

        setTimeout(() => {
            window.location.reload();

        }, 5000);
    }

}

