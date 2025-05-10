const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://smartlinkapi.imaginewebsite.com.tr/linkHub")
    .build();

connection.on("ReceiveLinkUpdate", (linkTitle) => {
    last5Queries();
});

connection.start()
    .then(() => {
        console.log("SignalR connection established successfully.");
        last5Queries();
    })
    .catch(err => {
        console.error("SignalR connection error: ", err);
    });
