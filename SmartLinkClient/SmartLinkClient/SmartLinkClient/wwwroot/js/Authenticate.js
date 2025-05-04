class AuthenticateManager{

    init(){

    }

    async isValidJwt()
    {
        const response = await fetch(`https://smartlinkapi.imaginewebsite.com.tr/api/users/profile/${localStorage.getItem("username")}`, {
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem("jwt")}`
            }
        }); 
        if (!response.ok) {
            return false;
        }
        return true;
    }
}