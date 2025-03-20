document.addEventListener('DOMContentLoaded', function() {
    // Retrieve stored data from content script (if available)
    chrome.storage.local.get(['ticketData'], function(result) {
        console.log('Ticket data retrieved', result);
        if (result.ticketData) {
            document.getElementById('title').value = result.ticketData.title || '';
            document.getElementById('description').value = result.ticketData.description || '';
            document.getElementById('timeSpent').value = result.ticketData.timeSpent || '';
        }

        const now = new Date();
        const year = now.getFullYear();
        const month = now.getMonth() + 1;
        const day = now.getDate();
        document.getElementById('ticketDate').value = `${year}-${month}-${day}`;
    });

    document.getElementById('send').addEventListener('click', function() {
        const title = document.getElementById('title').value;
        const description = document.getElementById('description').value;
        const timeSpent = document.getElementById('timeSpent').value;
        const ticketDate = document.getElementById('ticketDate').value;
        
        // Send data to background script
        chrome.runtime.sendMessage({
            type: "sendToAPI",
            data: {
                ticketId: title,
                ticketDescription: description,
                timeSpent: timeSpent,
                ticketDate: ticketDate
            }
        }, function(response) {
            console.log('Data sent to API', response);
        });
    });
});
