chrome.runtime.onMessage.addListener((request, sender, sendResponse) => {
    if (request.type === "sendToAPI") {
        fetch("http://localhost:5118/api/TicketTracking/add", { // change this to the actual API URL
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(request.data)
        })
            .then(response => response.json())
            .then(data => {
                console.log(data);
                sendResponse({ success: true, data: data });
            })
            .catch(error => {
                console.error('Error:', error);
                sendResponse({ success: false, error: error });
            });
        // Indicate that sendResponse will be called asynchronously
        return true;
    }
});
