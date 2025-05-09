class AuthenticateManager{

    init(){

    }

    async isValidJwt()
    {
        const jwt = localStorage.getItem("jwt")
        if (!jwt) {
            return false;
        }
        const response = await fetch(`https://smartlinkapi.imaginewebsite.com.tr/api/users/validate`, {
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${jwt}`
            }
        }); 
        if (!response.ok) {
            return false;
        }
        return true;
    }
}