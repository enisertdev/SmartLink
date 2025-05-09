class AuthenticateManager{

    init(){

    }

    async isValidJwt()
    {
        const username = localStorage.getItem("username")
        const jwt = localStorage.getItem("jwt")
        if (!username || !jwt) {
            return false;
        }
        const response = await fetch(`https://smartlinkapi.imaginewebsite.com.tr/api/users/profile/${username}`, {
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