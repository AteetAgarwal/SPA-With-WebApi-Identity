class AuthManager {
    constructor() {
        this.baseUrl = "https://localhost:7035/api/auth";
        this.isAuthenticated = false;
        this.userInfo = null;
    }

    async checkAuthStatus() {
        try {
            const res = await fetch(`${this.baseUrl}/is-authenticated`, {
                credentials: "include" // Include cookies
            });

            if (res.ok) {
                const data = await res.json();
                this.isAuthenticated = data.isAuthenticated;
                if (this.isAuthenticated) {
                    this.userInfo = data;
                }
                return data;
            }
            return { isAuthenticated: false };
        } catch (err) {
            console.error("Auth check failed", err);
            return { isAuthenticated: false };
        }
    }

    login() {
        window.location.href = `${this.baseUrl}/login`;
    }

    async logout() {
        try {
            await fetch(`${this.baseUrl}/logout`, {
                method: "POST",
                credentials: "include"
            });
            this.isAuthenticated = false;
            this.userInfo = null;
            window.location.href = "/";
        } catch (err) {
            console.error("Logout failed", err);
        }
    }

    async getUserProfile() {
        if (!this.isAuthenticated) {
            throw new Error("User is not authenticated");
        }

        try {
            const res = await fetch(`${this.baseUrl}/profile`, {
                credentials: "include"
            });

            if (res.ok) {
                const profile = await res.json();
                return profile;
            } else if (res.status === 401) {
                this.isAuthenticated = false;
                this.userInfo = null;
                throw new Error("Session expired");
            }
        } catch (err) {
            console.error("Failed to fetch user profile", err);
            throw err;
        }
    }
}

// Global instance of AuthManager
const authManager = new AuthManager();

async function login() {
    authManager.login();
}

async function logout() {
    await authManager.logout();
}

async function getProfile() {
    const output = document.getElementById("output");
    output.textContent = "Loading profile...";

    try {
        const profile = await authManager.getUserProfile();
        output.textContent = JSON.stringify(profile, null, 2);
    } catch (err) {
        output.textContent = `Error: ${err.message}`;
    }
}

async function init() {
    const output = document.getElementById("output");
    output.textContent = "Checking authentication status...";

    try {
        const authStatus = await authManager.checkAuthStatus();
        if (authStatus.isAuthenticated) {
            output.textContent = `Welcome, ${authStatus.name || "User"}!`;
        } else {
            output.textContent = "You are not logged in.";
        }
    } catch (err) {
        output.textContent = `Error: ${err.message}`;
    }
}

window.onload = init;